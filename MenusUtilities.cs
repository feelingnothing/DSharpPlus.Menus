using System;
using DSharpPlus.Entities;
using Newtonsoft.Json;

namespace DSharpPlus.Menus
{
    internal class MenuButton
    {
        [JsonProperty(PropertyName = "mn", Required = Required.Always)]
        public Guid MenuId { get; init; }

        [JsonProperty(PropertyName = "btn", Required = Required.Always)]
        public Guid ButtonId { get; init; }
    }

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