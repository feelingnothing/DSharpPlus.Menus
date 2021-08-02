namespace DSharpPlus.Menus.Attributes
{
    public class ButtonAttribute : BaseButtonAttribute
    {
        protected internal ButtonAttribute(ButtonStyle style, string label, string? id = null, string? emoji = null)
            : base(style, label, id, emoji)
        {
        }
    }

    public class PrimaryButtonAttribute : ButtonAttribute
    {
        public PrimaryButtonAttribute(string label, string? emoji = null) : base(ButtonStyle.Primary, label, emoji: emoji)
        {
        }
    }

    public class SecondaryButtonAttribute : ButtonAttribute
    {
        public SecondaryButtonAttribute(string label, string? emoji = null) : base(ButtonStyle.Secondary, label, emoji: emoji)
        {
        }
    }

    public class DangerButtonAttribute : ButtonAttribute
    {
        public DangerButtonAttribute(string label, string? emoji = null) : base(ButtonStyle.Danger, label, emoji: emoji)
        {
        }
    }

    public class SuccessButtonAttribute : ButtonAttribute
    {
        public SuccessButtonAttribute(string label, string? emoji = null) : base(ButtonStyle.Success, label, emoji: emoji)
        {
        }
    }
}