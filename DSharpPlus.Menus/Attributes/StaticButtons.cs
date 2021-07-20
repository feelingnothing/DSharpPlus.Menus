﻿namespace DSharpPlus.Menus.Attributes
{
    public class StaticButtonAttribute : BaseButtonAttribute
    {
        public override string Id { get; }

        protected internal StaticButtonAttribute(string id, ButtonStyle style, string label, ButtonRow row = ButtonRow.First, bool disabled = false, string? emoji = null)
            : base(style, label, row, disabled, null, emoji)
        {
            Id = id;
        }
    }

    public class StaticPrimaryButtonAttribute : StaticButtonAttribute
    {
        public StaticPrimaryButtonAttribute(string id, string label, ButtonRow row = ButtonRow.First, bool disabled = false, string? emoji = null)
            : base(id, ButtonStyle.Primary, label, row, disabled, emoji)
        {
        }
    }

    public class StaticSecondaryButtonAttribute : StaticButtonAttribute
    {
        public StaticSecondaryButtonAttribute(string id, string label, ButtonRow row = ButtonRow.First, bool disabled = false, string? emoji = null)
            : base(id, ButtonStyle.Secondary, label, row, disabled, emoji)
        {
        }
    }

    public class StaticDangerButtonAttribute : StaticButtonAttribute
    {
        public StaticDangerButtonAttribute(string id, string label, ButtonRow row = ButtonRow.First, bool disabled = false, string? emoji = null)
            : base(id, ButtonStyle.Danger, label, row, disabled, emoji)
        {
        }
    }

    public class StaticSuccessButtonAttribute : StaticButtonAttribute
    {
        public StaticSuccessButtonAttribute(string id, string label, ButtonRow row = ButtonRow.First, bool disabled = false, string? emoji = null)
            : base(id, ButtonStyle.Success, label, row, disabled, emoji)
        {
        }
    }
}