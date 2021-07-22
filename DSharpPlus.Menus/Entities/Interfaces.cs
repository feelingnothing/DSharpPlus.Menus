using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DSharpPlus.Menus.Entities
{
    public interface IMenuObject
    {
        public bool Disabled { get; set; }
        public DiscordComponentEmoji? Emoji { get; set; }
    }

    public interface IMenuButton : IMenuObject
    {
        public string Label { get; set; }
        public ButtonPosition Location { get; set; }
        public ButtonPosition Row { get; set; }
    }

    public interface IStyledMenuButton : IMenuButton
    {
        public ButtonStyle Style { get; set; }
    }

    internal interface IClickableMenuButton : IStyledMenuButton
    {
        public string Id { get; }
        public Func<IClickableMenuButton, ComponentInteractionCreateEventArgs, Task> Callable { get; }
    }

    internal interface ILinkMenuButton : IMenuButton
    {
        public Uri Url { get; set; }
    }
}