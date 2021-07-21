namespace DSharpPlus.Menus.Attributes
{
    public class ButtonAttribute : BaseButtonAttribute
    {
        protected internal ButtonAttribute(ButtonStyle style, string label, ButtonPosition location = ButtonPosition.First, 
            ButtonPosition row = ButtonPosition.First, bool disabled = false, string? id = null, string? emoji = null)
            : base(style, label, location, row, disabled, id, emoji)
        {
        }
    }

    public class PrimaryButtonAttribute : ButtonAttribute
    {
        public PrimaryButtonAttribute(string label, ButtonPosition location = ButtonPosition.First, ButtonPosition row = ButtonPosition.First, bool disabled = false, string? emoji = null)
            : base(ButtonStyle.Primary, label, location, row, disabled, null, emoji)
        {
        }
    }

    public class SecondaryButtonAttribute : ButtonAttribute
    {
        public SecondaryButtonAttribute(string label, ButtonPosition location = ButtonPosition.First, ButtonPosition row = ButtonPosition.First, bool disabled = false, string? emoji = null)
            : base(ButtonStyle.Secondary, label, location, row, disabled, null, emoji)
        {
        }
    }

    public class DangerButtonAttribute : ButtonAttribute
    {
        public DangerButtonAttribute(string label, ButtonPosition location = ButtonPosition.First, ButtonPosition row = ButtonPosition.First, bool disabled = false, string? emoji = null)
            : base(ButtonStyle.Danger, label, location, row, disabled, null, emoji)
        {
        }
    }

    public class SuccessButtonAttribute : ButtonAttribute
    {
        public SuccessButtonAttribute(string label, ButtonPosition location = ButtonPosition.First, ButtonPosition row = ButtonPosition.First, bool disabled = false, string? emoji = null)
            : base(ButtonStyle.Success, label, location, row, disabled, null, emoji)
        {
        }
    }
}