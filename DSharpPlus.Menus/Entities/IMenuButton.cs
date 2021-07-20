using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DSharpPlus.Menus.Entities
{
    public interface IMenuButton
    {
        public string Id { get; }
        public ButtonStyle Style { get; }
        public Func<DiscordInteraction, Task> Callable { get; }
        public string Label { get; }
        public ButtonRow Row { get; }
        public bool Disabled { get; }
        public DiscordComponentEmoji? Emoji { get; }
    }
}