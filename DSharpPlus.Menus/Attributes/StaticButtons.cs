namespace DSharpPlus.Menus.Attributes
{
    public class StaticButtonAttribute : BaseButtonAttribute
    {
        protected internal StaticButtonAttribute(string id, ButtonStyle style, string label, string? emoji = null) : base(style, label, id, emoji)
        {
        }
    }

    public class StaticPrimaryButtonAttribute : StaticButtonAttribute
    {
        public StaticPrimaryButtonAttribute(string id, string label, string? emoji = null) : base(id, ButtonStyle.Primary, label, emoji)
        {
        }
    }

    public class StaticSecondaryButtonAttribute : StaticButtonAttribute
    {
        public StaticSecondaryButtonAttribute(string id, string label, string? emoji = null) : base(id, ButtonStyle.Secondary, label, emoji)
        {
        }
    }

    public class StaticDangerButtonAttribute : StaticButtonAttribute
    {
        public StaticDangerButtonAttribute(string id, string label, string? emoji = null) : base(id, ButtonStyle.Danger, label, emoji)
        {
        }
    }

    public class StaticSuccessButtonAttribute : StaticButtonAttribute
    {
        public StaticSuccessButtonAttribute(string id, string label, string? emoji = null) : base(id, ButtonStyle.Success, label, emoji)
        {
        }
    }
}