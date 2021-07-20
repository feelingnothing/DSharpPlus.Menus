using DSharpPlus.Entities;
using DSharpPlus.Menus.Entities;
using Newtonsoft.Json;

namespace DSharpPlus.Menus
{
    internal class MenuButton
    {
        [JsonProperty(PropertyName = "m", Required = Required.Always)]
        public string MenuId { get; init; } = null!;

        [JsonProperty(PropertyName = "b", Required = Required.Always)]
        public string ButtonId { get; init; } = null!;
    }

    public enum ButtonRow
    {
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4,
        Fifth = 5
    }

    public static class MenusUtilities
    {
        public static DiscordMessageBuilder AddMenu(this DiscordMessageBuilder builder, MenuBase menu)
        {
            builder.AddComponents(menu.Serialize());
            return builder;
        }

        public static DiscordFollowupMessageBuilder AddMenu(this DiscordFollowupMessageBuilder builder, MenuBase menu)
        {
            builder.AddComponents(menu.Serialize());
            return builder;
        }

        public static DiscordWebhookBuilder AddMenu(this DiscordWebhookBuilder builder, MenuBase menu)
        {
            builder.AddComponents(menu.Serialize());
            return builder;
        }

        public static T GetStaticMenu<T>(this DiscordClient client) where T : StaticMenu => client.GetMenus().GetStaticMenu<T>();
        public static bool TryGetStaticMenu<T>(this DiscordClient client, out StaticMenu? menu) where T : StaticMenu => client.GetMenus().TryGetStaticMenu(out menu);
    }
}