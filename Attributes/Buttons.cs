using System;
using DSharpPlus.Entities;

namespace DSharpPlus.Menus.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ButtonAttribute : Attribute
    {
        public ButtonStyle Style { get; }
        public string Label { get; }
        public int Row { get; }
        public DiscordComponentEmoji? Emoji { get; }
        public bool Disabled { get; }

        public ButtonAttribute(ButtonStyle style, string label, int row = 0, bool disabled = false, string? emoji = null)
        {
            Style = style;
            Label = label;
            Row = row;
            Emoji = emoji != null ? new DiscordComponentEmoji(emoji) : null;
            Disabled = disabled;
        }
    }

    public class PrimaryButtonAttribute : ButtonAttribute
    {
        public PrimaryButtonAttribute(string label, int row = 0, bool disabled = false, string? emoji = null) : base(ButtonStyle.Primary, label, row, disabled, emoji)
        {
        }
    }

    public class SecondaryButtonAttribute : ButtonAttribute
    {
        public SecondaryButtonAttribute(string label, int row = 0, bool disabled = false, string? emoji = null) : base(ButtonStyle.Secondary, label, row, disabled, emoji)
        {
        }
    }

    public class DangerButtonAttribute : ButtonAttribute
    {
        public DangerButtonAttribute(string label, int row = 0, bool disabled = false, string? emoji = null) : base(ButtonStyle.Danger, label, row, disabled, emoji)
        {
        }
    }

    public class SuccessButtonAttribute : ButtonAttribute
    {
        public SuccessButtonAttribute(string label, int row = 0, bool disabled = false, string? emoji = null) : base(ButtonStyle.Success, label, row, disabled, emoji)
        {
        }
    }
}