using System;
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
        private TaskCompletionSource<bool?> Tcs { get; } = new();

        private ConfirmationMenu(ulong member, DiscordClient client, TimeSpan? timeout = null) : base(client, timeout) =>
            this.member = member;

        public override Task<bool> CanBeExecuted(ComponentInteractionCreateEventArgs args) =>
            Task.FromResult(args.User.Id == member);

        private async Task SetValue(ComponentInteractionCreateEventArgs args, bool value)
        {
            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            Tcs.TrySetResult(value);
            await StopAsync();
        }

        public static async Task<bool?> AskAsync(DiscordMessageBuilder builder, CommandContext ctx, TimeSpan? timeout = null, ulong? member = null)
        {
            member ??= ctx.Member.Id;
            var menu = new ConfirmationMenu(member.Value, ctx.Client, timeout);
            await menu.StartAsync();
            await ctx.RespondAsync(builder.AddMenu(menu));
            return await menu.Tcs.Task.ConfigureAwait(false);
        }

        [SuccessButton("Confirm")]
        public async Task ConfirmAsync(ComponentInteractionCreateEventArgs args) => await SetValue(args, true);

        [DangerButton("Deny")]
        public async Task DenyAsync(ComponentInteractionCreateEventArgs args) => await SetValue(args, false);

        public override Task StopAsync(bool timeout = false)
        {
            if (timeout) Tcs.TrySetResult(null);
            return base.StopAsync(timeout);
        }
    }

    [Group("menu")]
    public class SimpleCommandModule : BaseCommandModule
    {
        [GroupCommand]
        public async Task SomethingImportantAsync(CommandContext ctx)
        {
            var builder = new DiscordMessageBuilder().AddEmbed(
                new DiscordEmbedBuilder().WithColor(DiscordColor.Red).WithDescription("Really ***important*** action!\nDo you confirm it?"));
            var result = await ConfirmationMenu.AskAsync(builder, ctx, TimeSpan.FromMinutes(1));
            if (result is null) await ctx.RespondAsync("Your time have ran up, count as denied");
            else await ctx.RespondAsync(result.Value ? "You have confirmed this action!" : "You have denied this action.");
        }
    }
}