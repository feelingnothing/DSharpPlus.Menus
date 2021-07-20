namespace DSharpPlus.Menus
{
    public static class ExtensionMethods
    {
        public static MenusExtension UseMenus(this DiscordClient client)
        {
            var ext = new MenusExtension();
            client.AddExtension(ext);
            return ext;
        }

        public static MenusExtension GetMenus(this DiscordClient client) => client.GetExtension<MenusExtension>();
    }
}