using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using DSharpPlus.Menus.Entities;

namespace DSharpPlus.Menus
{
    public class MenusExtension : BaseExtension
    {
        private readonly ConcurrentDictionary<string, StaticMenu> pendingStaticMenus = new();
        private ComponentEventWaiter componentEventWaiter = null!;

        // Constant prefix would allow to distinguish menus from this extension and others, this would give the waiter to ack buttons that are not found
        // Must be as small as possible to allow custom ids to be bigger, means DSharpMenus
        internal const string IdPrefix = "DSM";

        // Prefixes to keep track of the menu type, so we dont apply respond behaviour to static menus
        internal const char RegularMenuPrefix = 'R';
        internal const char StaticMenuPrefix = 'S';

        public MenusConfiguration Configuration { get; }
        public MenusExtension(MenusConfiguration configuration) => Configuration = configuration;

        protected override void Setup(DiscordClient client)
        {
            if (Client is not null) throw new InvalidOperationException();
            Client = client;
            componentEventWaiter = new ComponentEventWaiter(Client, Configuration);
            Client.ComponentInteractionCreated += HandleStaticMenuInteraction;
        }

        internal async Task<(ComponentInteractionCreateEventArgs, MenuButtonDescriptor)?> WaitForMenuButton(MenuBase menu, CancellationToken? token = null)
        {
            token ??= new CancellationTokenSource(Configuration.DefaultMenuTimeout).Token;
            var result = await componentEventWaiter
                .WaitForMatchAsync(new ComponentMatchRequest(menu.Id,
                    (_, d) => menu.Buttons.OfType<IClickableMenuButton>().Any(b => b.Id == d.ButtonId), token.Value))
                .ConfigureAwait(false);
            return result;
        }

        // Just to clarify, we are using old implementation of the buttons callbacks because new one was created only for accounting to timeouts
        // Static menus do not have timeouts so we can rely on this solution
        private Task HandleStaticMenuInteraction(DiscordClient sender, ComponentInteractionCreateEventArgs args)
        {
            var id = args.Interaction.Data.CustomId;
            if (id.Length < IdPrefix.Length || id[..IdPrefix.Length] != IdPrefix || id[IdPrefix.Length] != StaticMenuPrefix)
                return Task.CompletedTask;

            var response = id[(IdPrefix.Length + 1)..].ParseJson<MenuButtonDescriptor>();
            if (response is null || !pendingStaticMenus.TryGetValue(response.MenuId, out var menu)) return Task.CompletedTask;
            if (menu.Buttons.OfType<IClickableMenuButton>().FirstOrDefault(b => b.Id == response.ButtonId) is not { } button) return Task.CompletedTask;
            return Task.Run(async () => await (await menu.CanBeExecuted(args) ? button.Callable.Invoke(button, args) : Task.CompletedTask));
        }

        /// <typeparam name="T">The generic type of static menu you want to get</typeparam>
        /// <returns>The static menu specified</returns>
        /// <exception cref="ArgumentException">If menu is not found</exception>
        public T GetStaticMenu<T>() where T : StaticMenu
        {
            if (pendingStaticMenus.FirstOrDefault(m => m.Value.GetType() == typeof(T)) is not ({ }, { } menu))
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
            if (pendingStaticMenus.Any(m => m.Value.GetType() == menu.GetType()))
                throw new ArgumentException("This menu is already registered");
            pendingStaticMenus[menu.Id] = menu;
        }
    }
}