using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DSharpPlus.Menus.Entities
{
    internal class ClickableMenuButton : IClickableMenuButton
    {
        public ClickableMenuButton(ButtonStyle style, Func<ButtonContext, Task> callable, string label,
            ButtonPosition location = ButtonPosition.First, ButtonPosition row = 0, bool disabled = false, DiscordComponentEmoji? emoji = null)
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
        public ButtonStyle Style { get; set; }
        public Func<ButtonContext, Task> Callable { get; }
        public string Label { get; set; }
        public ButtonPosition Location { get; set; }
        public ButtonPosition Row { get; set; }
        public bool Disabled { get; set; }
        public DiscordComponentEmoji? Emoji { get; set; }
    }

    internal class StaticMenuButton : IClickableMenuButton
    {
        public StaticMenuButton(string id, ButtonStyle style, Func<ButtonContext, Task> callable, string label,
            ButtonPosition location = ButtonPosition.First, ButtonPosition row = ButtonPosition.First, bool disabled = false, DiscordComponentEmoji? emoji = null)
        {
            Id = id;
            Style = style;
            Callable = callable;
            Label = label;
            Location = location;
            Row = row;
            Disabled = disabled;
            Emoji = emoji;
        }

        public string Id { get; set; }
        public ButtonStyle Style { get; set; }
        public Func<ButtonContext, Task> Callable { get; }
        public string Label { get; set; }
        public ButtonPosition Location { get; set; }
        public ButtonPosition Row { get; set; }
        public bool Disabled { get; set; }
        public DiscordComponentEmoji? Emoji { get; set; }
    }

    public class LinkMenuButton : ILinkMenuButton
    {
        public LinkMenuButton(string label, Uri url, ButtonPosition location = ButtonPosition.First,
            ButtonPosition row = ButtonPosition.First, bool disabled = false, DiscordComponentEmoji? emoji = null)
        {
            Label = label;
            Url = url;
            Location = location;
            Row = row;
            Disabled = disabled;
            Emoji = emoji;
        }

        public string Label { get; set; }
        public Uri Url { get; set; }
        public ButtonPosition Location { get; set; }
        public ButtonPosition Row { get; set; }
        public bool Disabled { get; set; }
        public DiscordComponentEmoji? Emoji { get; set; }
    }
}