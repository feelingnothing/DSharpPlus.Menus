using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;

namespace DSharpPlus.Menus
{
    public class MenusExtension : BaseExtension
    {
        public MenusConfiguration Configuration { get; }

        internal static readonly ConcurrentDictionary<Guid, Menu> PendingMenus = new();

        public MenusExtension(MenusConfiguration configuration) => Configuration = configuration;

        protected override void Setup(DiscordClient client)
        {
            Client = client;
            Client.ComponentInteractionCreated += HandleMenuInteraction;
        }

        private async Task HandleMenuInteraction(DiscordClient sender, ComponentInteractionCreateEventArgs args)
        {
            var prefix = Configuration.ComponentPrefix;
            string[] ids = args.Id.Split(' ');
            if (ids.Length != 3 || ids[0] != prefix) return;

            Guid menuId;
            Guid buttonId;
            try
            {
                menuId = Guid.Parse(ids[1]);
                buttonId = Guid.Parse(ids[2]);
            }
            catch (FormatException)
            {
                if (!Configuration.DisableParseFailureWarnings)
                    Client.Logger.LogWarning("Failed to format a component id, are you using message components somewhere else?\n" +
                                             "To disable this warning switch DisableParseFailureWarnings to true in configuration");
                return;
            }

            if (!PendingMenus.TryGetValue(menuId, out var menu)) return;
            if (menu.Buttons.FirstOrDefault(b => b.Id == buttonId) is not { } button) return;
            await button.Callable(args.Interaction);
        }
    }
}