namespace DSharpPlus.Menus.Attributes
{
    public class StaticButtonAttribute : BaseButtonAttribute
    {
        public override string Id { get; }

        protected internal StaticButtonAttribute(string id, ButtonStyle style, string label, ButtonPosition location = ButtonPosition.First,
            ButtonPosition row = ButtonPosition.First, bool disabled = false, string? emoji = null)
            : base(style, label, location, row, disabled, null, emoji)
        {
            Id = id;
        }
    }

    public class StaticPrimaryButtonAttribute : StaticButtonAttribute
    {
        public StaticPrimaryButtonAttribute(string id, string label, ButtonPosition location = ButtonPosition.First,
            ButtonPosition row = ButtonPosition.First, bool disabled = false, string? emoji = null)
            : base(id, ButtonStyle.Primary, label, location, row, disabled, emoji)
        {
        }
    }

    public class StaticSecondaryButtonAttribute : StaticButtonAttribute
    {
        public StaticSecondaryButtonAttribute(string id, string label, ButtonPosition location = ButtonPosition.First,
            ButtonPosition row = ButtonPosition.First, bool disabled = false, string? emoji = null)
            : base(id, ButtonStyle.Secondary, label, location, row, disabled, emoji)
        {
        }
    }

    public class StaticDangerButtonAttribute : StaticButtonAttribute
    {
        public StaticDangerButtonAttribute(string id, string label, ButtonPosition location = ButtonPosition.First,
            ButtonPosition row = ButtonPosition.First, bool disabled = false, string? emoji = null)
            : base(id, ButtonStyle.Danger, label, location, row, disabled, emoji)
        {
        }
    }

    public class StaticSuccessButtonAttribute : StaticButtonAttribute
    {
        public StaticSuccessButtonAttribute(string id, string label, ButtonPosition location = ButtonPosition.First,
            ButtonPosition row = ButtonPosition.First, bool disabled = false, string? emoji = null)
            : base(id, ButtonStyle.Success, label, location, row, disabled, emoji)
        {
        }
    }
}