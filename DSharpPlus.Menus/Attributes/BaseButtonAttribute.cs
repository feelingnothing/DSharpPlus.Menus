using System;
using DSharpPlus.Entities;

namespace DSharpPlus.Menus.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class BaseButtonAttribute : Attribute
    {
        public string Id { get; }
        public ButtonStyle Style { get; }
        public string Label { get; }
        public ButtonPosition Location { get; init; } = ButtonPosition.First;
        public ButtonPosition Row { get; init; } = ButtonPosition.First;
        public DiscordComponentEmoji? Emoji { get; init; } = null;
        public bool Disabled { get; init; } = false;

        protected internal BaseButtonAttribute(ButtonStyle style, string label, string? id = null)
        {
            if (id?.Length > 40) throw new ArgumentException("Id of the button must be maximum of 40 characters");
            Id = id ?? Guid.NewGuid().ToString();
            Style = style;
            Label = label;
        }
    }
}