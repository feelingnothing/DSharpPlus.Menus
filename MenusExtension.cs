using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
            MenuButton? response;
            try
            {
                response = JsonConvert.DeserializeObject<MenuButton>(args.Id);
                if (response is null) throw new JsonSerializationException();
            }
            catch (JsonSerializationException)
            {
                if (!Configuration.DisableParseFailureWarnings)
                    Client.Logger.LogError("Failed to format a component id, are you using message components somewhere else?\n" +
                                           "To disable this warning switch DisableParseFailureWarnings to true in configuration");
                return;
            }

            if (!PendingMenus.TryGetValue(response.MenuId, out var menu)) return;
            if (menu.Buttons.FirstOrDefault(b => b.Id == response.ButtonId) is not { } button) return;
            await button.Callable(args.Interaction);
        }
    }
}