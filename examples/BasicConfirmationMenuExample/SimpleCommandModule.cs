using System;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Menus;
using DSharpPlus.Menus.Attributes;
using DSharpPlus.Menus.Entities;

namespace BasicConfirmationMenuExample
{
    public class ConfirmationMenu : Menu
    {
        private readonly ulong member;
        public TaskCompletionSource<bool> Tcs { get; } = new();

        public ConfirmationMenu(ulong member, TimeSpan timeout, DiscordClient client) : base(client)
        {
            var token = new CancellationTokenSource(timeout);
            token.Token.Register(async () => await StopAsync(true));
            this.member = member;
        }

        public override Task<bool> CanBeExecuted(ComponentInteractionCreateEventArgs args) =>
            Task.FromResult(args.User.Id == member);

        private async void SetValue(ComponentInteractionCreateEventArgs args, bool value)
        {
            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            Tcs.TrySetResult(value);
            await StopAsync();
        }

        [SuccessButton("Confirm")]
        public async Task ConfirmAsync(ComponentInteractionCreateEventArgs args) => SetValue(args, true);

        [DangerButton("Deny")]
        public async Task DenyAsync(ComponentInteractionCreateEventArgs args) => SetValue(args, false);

        public override Task StopAsync(bool timeout = false)
        {
            if (timeout) Tcs.TrySetResult(false);
            return base.StopAsync(timeout);
        }
    }

    [Group("menu")]
    public class SimpleCommandModule : BaseCommandModule
    {
        [GroupCommand]
        public async Task SomethingImportantAsync(CommandContext ctx)
        {
            var menu = new ConfirmationMenu(ctx.Member.Id, TimeSpan.FromSeconds(10), ctx.Client);
            await menu.StartAsync();
            await ctx.RespondAsync(new DiscordMessageBuilder().AddMenu(menu).WithContent("Are you confirming this action?"));
            var result = await menu.Tcs.Task;
            await ctx.RespondAsync(result ? "You have confirmed this action!" : "You have denied this action.");
        }
    }
}