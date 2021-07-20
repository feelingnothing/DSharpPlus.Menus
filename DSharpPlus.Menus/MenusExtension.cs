using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using DSharpPlus.Menus.Entities;
using Newtonsoft.Json;

namespace DSharpPlus.Menus
{
    public class MenusExtension : BaseExtension
    {
        internal readonly ConcurrentDictionary<string, StaticMenu> PendingStaticMenus = new();
        internal readonly ConcurrentDictionary<string, Menu> PendingMenus = new();

        protected override void Setup(DiscordClient client)
        {
            if (Client is not null) throw new InvalidOperationException();
            Client = client;
            Client.ComponentInteractionCreated += HandleMenuInteraction;
            Client.ComponentInteractionCreated += HandleStaticMenuInteraction;
        }

        private Task HandleStaticMenuInteraction(DiscordClient sender, InteractionCreateEventArgs args)
        {
            var response = JsonConvert.DeserializeObject<MenuButton>(args.Interaction.Data.CustomId);
            if (response is null) return Task.CompletedTask;
            if (!PendingStaticMenus.TryGetValue(response.MenuId, out var menu)) return Task.CompletedTask;
            if (menu.Buttons.Find(b => b.Id == response.ButtonId) is not { } button) return Task.CompletedTask;
            return Task.Run(async () => await (await menu.CanBeExecuted(args.Interaction) ? button.Callable.Invoke(args.Interaction) : Task.CompletedTask));
        }

        private Task HandleMenuInteraction(DiscordClient sender, InteractionCreateEventArgs args)
        {
            var response = JsonConvert.DeserializeObject<MenuButton>(args.Interaction.Data.CustomId);
            if (response is null) return Task.CompletedTask;
            if (!PendingMenus.TryGetValue(response.MenuId, out var menu)) return Task.CompletedTask;
            if (menu.Buttons.Find(b => b.Id == response.ButtonId) is not { } button) return Task.CompletedTask;
            return Task.Run(async () => await (await menu.CanBeExecuted(args.Interaction) ? button.Callable.Invoke(args.Interaction) : Task.CompletedTask));
        }

        /// <typeparam name="T">The generic type of static menu you want to get</typeparam>
        /// <returns>The static menu specified</returns>
        /// <exception cref="ArgumentException">If menu is not found</exception>
        public T GetStaticMenu<T>() where T : StaticMenu
        {
            if (PendingStaticMenus.FirstOrDefault(m => m.Value.GetType() == typeof(T)) is not ({ }, { } menu))
                throw new ArgumentException("This menu is not registered");
            return (T) menu;
        }

        /// <param name="menu">Returned menu</param>
        /// <typeparam name="T">The generic type of static menu you want to get</typeparam>
        /// <returns>The static menu if found</returns>
        public bool TryGetStaticMenu<T>(out T? menu) where T : StaticMenu
        {
            menu = null;
            try
            {
                menu = GetStaticMenu<T>();
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        /// <summary>
        /// Registers your static menu
        /// </summary>
        /// <param name="predicate">Create predicate</param>
        /// <typeparam name="T">Static menu</typeparam>
        /// <exception cref="ArgumentException">There is already a registered menu with the same type</exception>
        public void RegisterStaticMenu<T>(Func<T> predicate) where T : StaticMenu
        {
            var menu = predicate();
            if (PendingStaticMenus.Any(m => m.Value.GetType() == menu.GetType()))
                throw new ArgumentException("This menu is already registered");
            PendingStaticMenus[menu.Id] = menu;
        }
    }
}