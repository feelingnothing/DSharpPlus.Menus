namespace DSharpPlus.Menus.Attributes
{
    public class ButtonAttribute : BaseButtonAttribute
    {
        protected internal ButtonAttribute(ButtonStyle style, string label, string? id = null, string? emoji = null)
            : base(style, label, id)
        {
        }
    }

    public class PrimaryButtonAttribute : ButtonAttribute
    {
        public PrimaryButtonAttribute(string label, string? emoji = null) : base(ButtonStyle.Primary, label)
        {
        }
    }

    public class SecondaryButtonAttribute : ButtonAttribute
    {
        public SecondaryButtonAttribute(string label) : base(ButtonStyle.Secondary, label)
        {
        }
    }

    public class DangerButtonAttribute : ButtonAttribute
    {
        public DangerButtonAttribute(string label) : base(ButtonStyle.Danger, label)
        {
        }
    }

    public class SuccessButtonAttribute : ButtonAttribute
    {
        public SuccessButtonAttribute(string label) : base(ButtonStyle.Success, label)
        {
        }
    }
}