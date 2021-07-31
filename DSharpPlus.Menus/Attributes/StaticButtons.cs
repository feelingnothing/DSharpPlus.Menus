namespace DSharpPlus.Menus.Attributes
{
    public class StaticButtonAttribute : BaseButtonAttribute
    {
        protected internal StaticButtonAttribute(string id, ButtonStyle style, string label) : base(style, label, id)
        {
        }
    }

    public class StaticPrimaryButtonAttribute : StaticButtonAttribute
    {
        public StaticPrimaryButtonAttribute(string id, string label) : base(id, ButtonStyle.Primary, label)
        {
        }
    }

    public class StaticSecondaryButtonAttribute : StaticButtonAttribute
    {
        public StaticSecondaryButtonAttribute(string id, string label) : base(id, ButtonStyle.Secondary, label)
        {
        }
    }

    public class StaticDangerButtonAttribute : StaticButtonAttribute
    {
        public StaticDangerButtonAttribute(string id, string label) : base(id, ButtonStyle.Danger, label)
        {
        }
    }

    public class StaticSuccessButtonAttribute : StaticButtonAttribute
    {
        public StaticSuccessButtonAttribute(string id, string label) : base(id, ButtonStyle.Success, label)
        {
        }
    }
}