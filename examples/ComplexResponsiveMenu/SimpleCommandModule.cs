using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Menus;
using DSharpPlus.Menus.Attributes;
using DSharpPlus.Menus.Entities;

namespace ComplexResponsiveMenu
{
    public class MyStaticMenu : StaticMenu
    {
        public MyStaticMenu(DiscordClient client) : base("best_static_menu_id", client)
        {
        }

        [StaticSecondaryButton("best_static_button_id", "Create menu")]
        public async Task SendMenuAsync(ComponentInteractionCreateEventArgs args)
        {
            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            var menu = new MyMenu(Client, true);
            await menu.StartAsync();
            await args.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder()
                .WithContent("Only for you my guy!").AsEphemeral(true).AddMenu(menu));
        }
    }

    public class MyMenu : Menu
    {
        private readonly bool createdByStatic;

        public MyMenu(DiscordClient client, bool createdByStatic) : base(client) =>
            this.createdByStatic = createdByStatic;

        [SecondaryButton("Are you sure this is menu only for me?")]
        public async Task ConfirmAsync(ComponentInteractionCreateEventArgs args)
        {
            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            await args.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().WithContent(createdByStatic ? "Yes i'm sure." : "No i'm not!").AddMenu(this));
        }
    }

    [Group("menu")]
    public class SimpleCommandModule : BaseCommandModule
    {
        [GroupCommand]
        public async Task SendMenuAsync(CommandContext ctx)
        {
            var menu = new MyMenu(ctx.Client, false);
            await menu.StartAsync();
            await ctx.RespondAsync(new DiscordMessageBuilder().AddMenu(menu).WithContent("Your menu, but not only for you."));
        }

        // You do not want poor people messing with this
        [Command("send"), RequireOwner]
        public async Task SendStaticMenuAsync(CommandContext ctx)
        {
            var menu = ctx.Client.GetStaticMenu<MyStaticMenu>();
            await ctx.RespondAsync(new DiscordMessageBuilder().WithContent("Static menu that creates private menus only by click of the button!").AddMenu(menu));
        }
    }
}