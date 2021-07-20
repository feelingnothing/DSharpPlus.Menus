using System;
using DSharpPlus.Entities;

namespace DSharpPlus.Menus.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class BaseButtonAttribute : Attribute
    {
        public virtual string? Id { get; }
        public ButtonStyle Style { get; }
        public string Label { get; }
        public ButtonRow Row { get; }
        public DiscordComponentEmoji? Emoji { get; }
        public bool Disabled { get; }

        protected internal BaseButtonAttribute(ButtonStyle style, string label, ButtonRow row = ButtonRow.First, bool disabled = false, string? id = null, string? emoji = null)
        {
            if (id?.Length > 42) throw new ArgumentException("Id of the button must be maximum of 32 characters");
            Id = id;
            Style = style;
            Label = label;
            Row = row;
            Emoji = emoji != null ? new DiscordComponentEmoji(emoji) : null;
            Disabled = disabled;
        }
    }
}