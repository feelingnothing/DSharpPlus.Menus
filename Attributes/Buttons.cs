using System;

namespace DSharpPlus.Menus.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ButtonAttribute : Attribute
    {
        internal Guid Id = Guid.NewGuid();
        public ButtonStyle Style { get; }
        public string Label { get; }
        public bool Disabled { get; }

        public ButtonAttribute(ButtonStyle style, string label, bool disabled = false)
        {
            Style = style;
            Label = label;
            Disabled = disabled;
        }
    }

    public class PrimaryButtonAttribute : ButtonAttribute
    {
        public PrimaryButtonAttribute(string label, bool disabled = false) : base(ButtonStyle.Primary, label, disabled)
        {
        }
    }

    public class SecondaryButtonAttribute : ButtonAttribute
    {
        public SecondaryButtonAttribute(string label, bool disabled = false) : base(ButtonStyle.Secondary, label, disabled)
        {
        }
    }

    public class DangerButtonAttribute : ButtonAttribute
    {
        public DangerButtonAttribute(string label, bool disabled = false) : base(ButtonStyle.Danger, label, disabled)
        {
        }
    }

    public class SuccessButtonAttribute : ButtonAttribute
    {
        public SuccessButtonAttribute(string label, bool disabled = false) : base(ButtonStyle.Success, label, disabled)
        {
        }
    }
}