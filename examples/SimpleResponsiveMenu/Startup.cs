using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Menus;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleResponsiveMenu
{
    public static class Startup
    {
        private static readonly string Token = Environment.GetEnvironmentVariable("TOKEN")!;
        private static readonly string[] Prefixes = {".", "~", "\\"};

        public static async Task Main()
        {
            var services = ConfigureServices();
            var client = services.GetRequiredService<DiscordClient>();
            var commands = services.GetRequiredService<CommandsNextExtension>();

            client.UseMenus(new MenusConfiguration
            {
                DefaultMenuTimeout = TimeSpan.FromMinutes(1)
            });

            commands.RegisterCommands<SimpleCommandModule>();

            await client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_ => new DiscordClient(new DiscordConfiguration
                {
                    Token = Token,
                    Intents = DiscordIntents.AllUnprivileged
                }))
                .AddSingleton(s =>
                {
                    var client = s.GetRequiredService<DiscordClient>();
                    return client.UseCommandsNext(new CommandsNextConfiguration
                    {
                        StringPrefixes = Prefixes,
                        Services = s
                    });
                })
                .BuildServiceProvider(true);
        }
    }
}