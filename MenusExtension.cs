﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;

namespace DSharpPlus.Menus
{
    public class MenusExtension : BaseExtension
    {
        public MenusConfiguration Configuration { get; }

        internal static readonly ConcurrentDictionary<Guid, Menu> PendingMenus = new();

        internal MenusExtension(MenusConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void Setup(DiscordClient client)
        {
            client.ComponentInteractionCreated += HandleMenuInteraction;
        }

        private Task HandleMenuInteraction(DiscordClient sender, ComponentInteractionCreateEventArgs args)
        {
            ReadOnlySpan<char> response = args.Id.ToCharArray();
            var menuId = Guid.Parse(response[..38]);
            var buttonId = Guid.Parse(response[38..]);
            if (!PendingMenus.TryGetValue(menuId, out var menu)) return Task.CompletedTask;
            if (menu.Buttons.FirstOrDefault(b => b.Id == buttonId) is not { } button) return Task.CompletedTask;
            Task.Run(async () => await button.Callable(args.Interaction));
            return Task.CompletedTask;
        }
    }
}