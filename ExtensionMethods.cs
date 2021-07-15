namespace DSharpPlus.Menus
{
    public static class ExtensionMethods
    {
        public static MenusExtension UseMenus(this DiscordClient client, MenusConfiguration configuration)
        {
            var ext = new MenusExtension(client, configuration);
            client.AddExtension(ext);
            return ext;
        }

        public static MenusExtension GetMenus(this DiscordClient client) => client.GetExtension<MenusExtension>();
    }
}