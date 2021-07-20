using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Menus;
using DSharpPlus.Menus.Attributes;
using DSharpPlus.Menus.Entities;
using Microsoft.Extensions.Logging;

namespace SimpleResponsiveMenu
{
    public class MyMenu : Menu
    {
        private readonly ILogger logger;

        public MyMenu(DiscordClient client) : base(client) =>
            logger = client.Logger;

        public override Task StartAsync()
        {
            logger.LogInformation("MyMenu is starting, omg!");
            return base.StartAsync();
        }

        public override Task StopAsync(bool timeout = false)
        {
            logger.LogInformation("MyMenu is finally stopping!");
            return base.StopAsync(timeout);
        }

        private async Task Interact(ComponentInteractionCreateEventArgs args, string type)
        {
            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            logger.LogWarning("{User} just pressed my button, do something!", args.Interaction.User.Username);
            await args.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().AddMenu(this).WithContent($"It's my {type} button, don't you dare touch it"));
        }

        [PrimaryButton("It's my primary button")]
        public async Task MyPrimaryButtonAsync(ComponentInteractionCreateEventArgs args) => await Interact(args, "primary");

        [SecondaryButton("It's secondary button")]
        public async Task MySecondaryButtonAsync(ComponentInteractionCreateEventArgs args) => await Interact(args, "secondary");

        [DangerButton("It's my danger button", ButtonRow.Second)]
        public async Task MyDangerButtonAsync(ComponentInteractionCreateEventArgs args) => await Interact(args, "danger");

        [SuccessButton("It's my success button", ButtonRow.Second)]
        public async Task MySuccessButtonAsync(ComponentInteractionCreateEventArgs args) => await Interact(args, "success");

        [SecondaryButton("Don't forget to create a close button", ButtonRow.Third)]
        public async Task CloseAsync(ComponentInteractionCreateEventArgs args)
        {
            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            await args.Interaction.DeleteOriginalResponseAsync();
            await StopAsync();
        }
    }

    [Group("menu")]
    public class SimpleCommandModule : BaseCommandModule
    {
        [GroupCommand]
        public async Task SendMenuAsync(CommandContext ctx)
        {
            var menu = new MyMenu(ctx.Client);
            await menu.StartAsync();
            await ctx.RespondAsync(new DiscordMessageBuilder().AddMenu(menu).WithContent("It's my button, do not touch it!!!"));
        }
    }
}