using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Menus.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DSharpPlus.Menus.Entities
{
    public enum MenuStatus
    {
        None = 0,
        Started = 1,
        Ended = 2,
    }

    public abstract class MenuBase
    {
        public DiscordClient Client { get; }
        internal readonly MenusExtension Extension;
        public string Id { get; }
        public TimeSpan TimeOutSpan { get; }
        public MenuStatus Status { get; protected internal set; } = MenuStatus.None;
        public IReadOnlyList<IMenuButton> Buttons { get; protected internal set; } = new List<IMenuButton>();
        internal protected CancellationTokenSource TokenSource { get; }

        protected internal MenuBase(DiscordClient client, string id, TimeSpan? timeout = null)
        {
            Id = id;
            Client = client;
            Extension = client.GetMenus();
            TimeOutSpan = timeout ?? Extension.Configuration.DefaultMenuTimeout;
            TokenSource = new CancellationTokenSource(TimeOutSpan);
        }

        protected internal async Task LoopAsync()
        {
            async Task ExecuteButton(IMenuButton button, ComponentInteractionCreateEventArgs args)
            {
                try
                {
                    if (await CanBeExecuted(args))
                        await button.Callable.Invoke(args);
                }
                catch (Exception e)
                {
                    Client.Logger.LogError(e, "Error while executing button");
                }
            }

            Status = MenuStatus.Started;
            while (!TokenSource.IsCancellationRequested)
            {
                var result = await Extension.WaitForMenuButton(this, TokenSource.Token);
                if (result is null)
                {
                    // Timed out
                    await StopAsync(true);
                    return;
                }

                var (args, ids) = result.Value;
                if (Buttons.FirstOrDefault(b => b.Id == ids.ButtonId) is not { } button) return;
                switch (Extension.Configuration.ButtonButtonCallback)
                {
                    case MenuButtonCallbackBehaviour.Asynchronous:
                        _ = Task.Run(async () => await ExecuteButton(button, args));
                        break;
                    case MenuButtonCallbackBehaviour.Synchronous:
                        await ExecuteButton(button, args);
                        break;
                }
            }
        }

        protected internal IEnumerable<(MethodInfo, T)> CollectInteractionMethodsWithAttribute<T>() where T : BaseButtonAttribute
        {
            return GetType().GetMethods().Select<MethodInfo, (MethodInfo, T)?>(m =>
            {
                if (!m.IsPublic || m.IsStatic || m.IsAbstract) return null;
                var parameters = m.GetParameters();
                if (parameters.Length is not 1) return null;
                var parameter = parameters.First();
                if (parameter.ParameterType != typeof(ComponentInteractionCreateEventArgs) || m.ReturnType != typeof(Task)) return null;
                var attr = m.GetCustomAttribute<T>(false);
                if (attr is null) return null;
                return (m, attr);
            }).Where(t => t.HasValue).Select(t => t!.Value);
        }

        internal IEnumerable<DiscordActionRowComponent> Serialize() => Buttons.GroupBy(b => b.Row)
            .Select(g => new DiscordActionRowComponent(g.OrderBy(b => b.Location).Select(b => new DiscordButtonComponent(
                b.Style, MenusExtension.IdPrefix + JsonConvert.SerializeObject(new Menus.MenuButton {MenuId = Id, ButtonId = b.Id}), b.Label, b.Disabled, b.Emoji))));

        public virtual Task<bool> CanBeExecuted(ComponentInteractionCreateEventArgs args) => Task.FromResult(true);
        public abstract Task StartAsync();
        public abstract Task StopAsync(bool timeout = false);
    }
}