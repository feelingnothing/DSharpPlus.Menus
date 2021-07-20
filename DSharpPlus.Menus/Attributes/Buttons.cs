namespace DSharpPlus.Menus.Attributes
{
    public class ButtonAttribute : BaseButtonAttribute
    {
        protected internal ButtonAttribute(ButtonStyle style, string label, ButtonRow row = ButtonRow.First, bool disabled = false, string? id = null, string? emoji = null)
            : base(style, label, row, disabled, id, emoji)
        {
        }
    }

    public class PrimaryButtonAttribute : ButtonAttribute
    {
        public PrimaryButtonAttribute(string label, ButtonRow row = ButtonRow.First, bool disabled = false, string? emoji = null)
            : base(ButtonStyle.Primary, label, row, disabled, null, emoji)
        {
        }
    }

    public class SecondaryButtonAttribute : ButtonAttribute
    {
        public SecondaryButtonAttribute(string label, ButtonRow row = ButtonRow.First, bool disabled = false, string? emoji = null)
            : base(ButtonStyle.Secondary, label, row, disabled, null, emoji)
        {
        }
    }

    public class DangerButtonAttribute : ButtonAttribute
    {
        public DangerButtonAttribute(string label, ButtonRow row = ButtonRow.First, bool disabled = false, string? emoji = null)
            : base(ButtonStyle.Danger, label, row, disabled, null, emoji)
        {
        }
    }

    public class SuccessButtonAttribute : ButtonAttribute
    {
        public SuccessButtonAttribute(string label, ButtonRow row = ButtonRow.First, bool disabled = false, string? emoji = null)
            : base(ButtonStyle.Success, label, row, disabled, null, emoji)
        {
        }
    }
}