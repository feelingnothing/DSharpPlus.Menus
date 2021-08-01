using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Menus.Attributes;
using Microsoft.Extensions.Logging;

namespace DSharpPlus.Menus.Entities
{
    public abstract class Menu : MenuBase
    {
        public Menu(DiscordClient client, TimeSpan? timeout = null) : base(client, Guid.NewGuid().ToString(), timeout)
        {
            foreach (var (info, attribute) in CollectInteractionMethodsWithAttribute<ButtonAttribute>())
                Buttons.Add(new ClickableMenuButton(attribute.Style, info.CreateDelegate<Func<ButtonContext, Task>>(this),
                    attribute.Label, attribute.Location, attribute.Row, attribute.Disabled, attribute.Emoji));
        }

        protected internal async Task LoopAsync()
        {
            async Task ExecuteButton(ButtonContext context)
            {
                try
                {
                    if (await CanBeExecuted(context))
                        await context.Button.Callable.Invoke(context);
                }
                catch (Exception e)
                {
                    Client.Logger.LogError(e, "Error while executing button");
                }
            }

            Status = MenuStatus.Started;
            while (!TokenSource.IsCancellationRequested)
            {
                var result = await Extension.WaitForMenuButton(this, TimeOutSpan);
                if (result is null)
                {
                    // Timed out
                    await StopAsync(true);
                    return;
                }

                var (args, ids) = result.Value;
                if (Buttons.OfType<IClickableMenuButton>().FirstOrDefault(b => b.Id == ids.ButtonId) is not { } button) return;
                var context = new ButtonContext {Button = button, Interaction = args.Interaction, Message = args.Message};
                switch (Extension.Configuration.ButtonButtonCallback)
                {
                    case MenuButtonCallbackBehaviour.Asynchronous:
                        _ = Task.Run(async () => await ExecuteButton(context));
                        break;
                    case MenuButtonCallbackBehaviour.Synchronous:
                        await ExecuteButton(context);
                        break;
                }
            }
        }

        /// <summary>
        /// Starts your menu for you, use it only once
        /// </summary>
        /// <exception cref="InvalidOperationException">If menu is already running</exception>
        public override Task StartAsync()
        {
            if (Status is MenuStatus.Started) throw new InvalidOperationException("This menu is already started");
            _ = Task.Run(async () => await LoopAsync());
            return Task.CompletedTask;
        }

        public override Task StopAsync(bool timeout = false)
        {
            if (Status is MenuStatus.Ended or MenuStatus.None) throw new InvalidOperationException("This menu is already stopped or has not started yet");
            TokenSource.Cancel();
            Status = MenuStatus.Ended;
            return Task.CompletedTask;
        }
    }
}