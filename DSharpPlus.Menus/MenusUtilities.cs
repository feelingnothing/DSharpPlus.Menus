using DSharpPlus.Entities;
using DSharpPlus.Menus.Entities;
using Newtonsoft.Json;

namespace DSharpPlus.Menus
{
    internal class MenuButtonDescriptor
    {
        [JsonProperty(PropertyName = "m", Required = Required.Always)]
        public string MenuId { get; init; } = null!;

        [JsonProperty(PropertyName = "b", Required = Required.Always)]
        public string ButtonId { get; init; } = null!;
    }

    public enum ButtonPosition
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

        public static DiscordInteractionResponseBuilder AddMenu(this DiscordInteractionResponseBuilder builder, MenuBase menu)
        {
            builder.AddComponents(menu.Serialize());
            return builder;
        }

        public static T GetStaticMenu<T>(this DiscordClient client) where T : StaticMenu => client.GetMenus().GetStaticMenu<T>();
        public static bool TryGetStaticMenu<T>(this DiscordClient client, out T? menu) where T : StaticMenu => client.GetMenus().TryGetStaticMenu(out menu);

        internal static T? ParseJson<T>(this string @this)
        {
            T? result;

            try
            {
                result = JsonConvert.DeserializeObject<T>(@this);
            }
            catch (JsonSerializationException)
            {
                result = default;
            }

            return result;
        }

        internal static string SerializeButton(this MenuBase menu, IClickableMenuButton button) =>
            MenusExtension.IdPrefix + (menu is StaticMenu ? MenusExtension.StaticMenuPrefix : MenusExtension.RegularMenuPrefix) +
            JsonConvert.SerializeObject(new MenuButtonDescriptor {MenuId = menu.Id, ButtonId = button.Id});
    }
}