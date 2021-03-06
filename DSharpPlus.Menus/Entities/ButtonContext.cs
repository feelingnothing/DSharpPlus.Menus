using DSharpPlus.Entities;

namespace DSharpPlus.Menus.Entities
{
    public class ButtonContext
    {
        internal ButtonContext(DiscordInteraction interaction, IClickableMenuButton button, DiscordMessage message, DiscordClient client)
        {
            Interaction = interaction;
            Button = button;
            Message = message;
            Client = client;
        }

        /// <summary>The user that invoked the menu.</summary>
        public DiscordInteraction Interaction { get; }

        /// <summary>Button used for this menu.</summary>
        public IClickableMenuButton Button { get; }

        /// <summary>The guild this menu was invoked in.</summary>
        public DiscordGuild Guild => Interaction.Guild;

        /// <summary>The channel this menu invoked in.</summary>
        public DiscordChannel Channel => Interaction.Channel;

        /// <summary>The message this menu attached to.</summary>
        public DiscordUser User => Interaction.User;

        /// <summary>The channel this menu was invoked in.</summary>
        public DiscordMessage Message { get; }

        /// <summary>The client this menu was invoked with.</summary>
        public DiscordClient Client { get; }
    }
}