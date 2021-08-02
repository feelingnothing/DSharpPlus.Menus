using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.Menus.Attributes;

namespace DSharpPlus.Menus.Entities
{
    public enum MenuStatus
    {
        None,
        Started,
        Ended
    }

    public abstract class MenuBase
    {
        public DiscordClient Client { get; }
        internal readonly MenusExtension Extension;
        public string Id { get; }
        public TimeSpan TimeOutSpan { get; }
        public MenuStatus Status { get; protected internal set; } = MenuStatus.None;
        internal ConcurrentBag<IMenuButton> Buttons { get; } = new();
        protected internal CancellationTokenSource TokenSource { get; }

        protected internal MenuBase(DiscordClient client, string id, TimeSpan? timeout = null)
        {
            Id = id;
            Client = client;
            Extension = client.GetMenus();
            TimeOutSpan = timeout ?? Extension.Configuration.DefaultMenuTimeout;
            TokenSource = new CancellationTokenSource(TimeOutSpan);
        }

        protected void AddLink(LinkMenuButton button) => Buttons.Add(button);

        protected internal IEnumerable<(MethodInfo, T)> CollectInteractionMethodsWithAttribute<T>() where T : BaseButtonAttribute
        {
            return GetType().GetMethods().Reverse().Select<MethodInfo, (MethodInfo, T)?>(m =>
            {
                if (!m.IsPublic || m.IsStatic || m.IsAbstract) return null;
                var parameters = m.GetParameters();
                if (parameters.Length is not 1) return null;
                if (parameters.First().ParameterType != typeof(ButtonContext) || m.ReturnType != typeof(Task)) return null;
                var attr = m.GetCustomAttribute<T>(false);
                if (attr is null) return null;
                return (m, attr);
            }).Where(t => t.HasValue).Select(t => t!.Value);
        }

        internal IEnumerable<DiscordActionRowComponent> Serialize()
        {
            return Buttons.GroupBy(b => b.Row).OrderBy(g => g.Key).Select(group =>
            {
                return new DiscordActionRowComponent(group.OrderBy(g => g.Location).Select(button =>
                {
                    DiscordComponent component = button switch
                    {
                        IClickableMenuButton c => new DiscordButtonComponent(c.Style, this.SerializeButton(c), c.Label, c.Disabled, c.Emoji),
                        ILinkMenuButton l => new DiscordLinkButtonComponent(l.Url.ToString(), l.Label, l.Disabled, l.Emoji),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    return component;
                }));
            });
        }

        public virtual Task<bool> CanBeExecuted(ButtonContext args) => Task.FromResult(true);
        public abstract Task StartAsync();
        public abstract Task StopAsync(bool timeout = false);
    }
}