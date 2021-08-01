namespace DSharpPlus.Menus
{
    public static class ExtensionMethods
    {
        public static MenusExtension UseMenus(this DiscordClient client, MenusConfiguration? configuration = null)
        {
            var ext = new MenusExtension(configuration ?? new MenusConfiguration());
            client.AddExtension(ext);
            return ext;
        }

        public static MenusExtension UseMenus(this DiscordShardedClient client, MenusConfiguration? configuration = null)
        {
            var ext = new MenusExtension(configuration ?? new MenusConfiguration());
            foreach (var (_, shard) in client.ShardClients)
                shard.AddExtension(ext);
            return ext;
        }

        public static MenusExtension GetMenus(this DiscordClient client) => client.GetExtension<MenusExtension>();
    }
}