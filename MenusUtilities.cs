using DSharpPlus.Entities;

namespace DSharpPlus.Menus
{
    public static class MenusUtilities
    {
        public static DiscordMessageBuilder AddMenu(this DiscordMessageBuilder builder, Menu menu)
        {
            builder.AddComponents(menu.Serialize());
            return builder;
        }

        public static DiscordFollowupMessageBuilder AddMenu(this DiscordFollowupMessageBuilder builder, Menu menu)
        {
            builder.AddComponents(menu.Serialize());
            return builder;
        }
    }
}