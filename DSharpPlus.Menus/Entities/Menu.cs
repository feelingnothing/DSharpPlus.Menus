using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Menus.Attributes;

namespace DSharpPlus.Menus.Entities
{
    internal class MenuButton : IMenuButton
    {
        public MenuButton(ButtonStyle style, Func<ComponentInteractionCreateEventArgs, Task> callable, string label, ButtonPosition location = ButtonPosition.First,
            ButtonPosition row = 0, bool disabled = false, DiscordComponentEmoji? emoji = null)
        {
            Style = style;
            Callable = callable;
            Label = label;
            Location = location;
            Row = row;
            Disabled = disabled;
            Emoji = emoji;
        }

        public string Id { get; } = Guid.NewGuid().ToString();
        public ButtonStyle Style { get; }
        public Func<ComponentInteractionCreateEventArgs, Task> Callable { get; }
        public string Label { get; set; }
        public ButtonPosition Location { get; }
        public ButtonPosition Row { get; }
        public bool Disabled { get; set; }
        public DiscordComponentEmoji? Emoji { get; set; }
    }

    public abstract class Menu : MenuBase
    {
        public Menu(DiscordClient client, TimeSpan? timeout = null) : base(client, Guid.NewGuid().ToString(), timeout) =>
            Buttons = CollectInteractionMethodsWithAttribute<ButtonAttribute>().ToList().Select(((MethodInfo i, ButtonAttribute a) t) => new MenuButton(t.a.Style,
                t.i.CreateDelegate<Func<ComponentInteractionCreateEventArgs, Task>>(this), t.a.Label, t.a.Location, t.a.Row, t.a.Disabled, t.a.Emoji)).ToList();

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