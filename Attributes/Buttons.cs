using System;
using DSharpPlus.Entities;

namespace DSharpPlus.Menus.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ButtonAttribute : Attribute
    {
        internal Guid Id = Guid.NewGuid();
        public ButtonStyle Style { get; }
        public string Label { get; }
        public DiscordComponentEmoji Emoji { get; }
        public bool Disabled { get; }

        public ButtonAttribute(ButtonStyle style, string label, bool disabled = false, string? emoji = null)
        {
            Style = style;
            Label = label;
            Emoji = new DiscordComponentEmoji(emoji);
            Disabled = disabled;
        }
    }

    public class PrimaryButtonAttribute : ButtonAttribute
    {
        public PrimaryButtonAttribute(string label, bool disabled = false, string? emoji = null) : base(ButtonStyle.Primary, label, disabled, emoji)
        {
        }
    }

    public class SecondaryButtonAttribute : ButtonAttribute
    {
        public SecondaryButtonAttribute(string label, bool disabled = false, string? emoji = null) : base(ButtonStyle.Secondary, label, disabled, emoji)
        {
        }
    }

    public class DangerButtonAttribute : ButtonAttribute
    {
        public DangerButtonAttribute(string label, bool disabled = false, string? emoji = null) : base(ButtonStyle.Danger, label, disabled, emoji)
        {
        }
    }

    public class SuccessButtonAttribute : ButtonAttribute
    {
        public SuccessButtonAttribute(string label, bool disabled = false, string? emoji = null) : base(ButtonStyle.Success, label, disabled, emoji)
        {
        }
    }
}