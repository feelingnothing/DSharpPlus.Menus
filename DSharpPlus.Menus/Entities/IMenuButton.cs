using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DSharpPlus.Menus.Entities
{
    public interface IMenuButton
    {
        public string Id { get; }
        public ButtonStyle Style { get; }
        public Func<ComponentInteractionCreateEventArgs, Task> Callable { get; }
        public string Label { get; }
        public ButtonPosition Location { get; }
        public ButtonPosition Row { get; }
        public bool Disabled { get; }
        public DiscordComponentEmoji? Emoji { get; }
    }
}