using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
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

        private async Task Interact(ButtonContext context, string type)
        {
            await context.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            logger.LogWarning("{User} just pressed my button, do something!", context.Interaction.User.Username);
            await context.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().AddMenu(this).WithContent($"It's my {type} button, don't you dare touch it"));
        }

        [PrimaryButton("It's my primary button")]
        public async Task MyPrimaryButtonAsync(ButtonContext context) => await Interact(context, "primary");

        [SecondaryButton("It's secondary button")]
        public async Task MySecondaryButtonAsync(ButtonContext context) => await Interact(context, "secondary");

        [DangerButton("It's my danger button", Row = ButtonPosition.Second)]
        public async Task MyDangerButtonAsync(ButtonContext context) => await Interact(context, "danger");

        [SuccessButton("It's my success button", Row = ButtonPosition.Second)]
        public async Task MySuccessButtonAsync(ButtonContext context) => await Interact(context, "success");

        [SecondaryButton("Don't forget to create a close button", Row = ButtonPosition.Third)]
        public async Task CloseAsync(ButtonContext context)
        {
            await context.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            await context.Interaction.DeleteOriginalResponseAsync();
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