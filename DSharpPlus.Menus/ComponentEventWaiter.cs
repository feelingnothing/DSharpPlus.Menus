using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

// Thanks to Velvet for this peace of code, without him i would not know how TaskCompletionSource works and even existed
// https://github.com/VelvetThePanda

namespace DSharpPlus.Menus
{
    internal class ComponentMatchRequest
    {
        public string Id { get; }
        public TaskCompletionSource<(ComponentInteractionCreateEventArgs, MenuButtonDescriptor)?> Tcs { get; } = new();
        private readonly Func<ComponentInteractionCreateEventArgs, MenuButtonDescriptor, bool> predicate;

        public ComponentMatchRequest(string id, Func<ComponentInteractionCreateEventArgs, MenuButtonDescriptor, bool> predicate, CancellationToken token)
        {
            Id = id;
            this.predicate = predicate;
            token.Register(() => Tcs.TrySetResult(null));
        }

        public bool IsMatch(ComponentInteractionCreateEventArgs interaction, MenuButtonDescriptor button) => predicate(interaction, button);
    }

    internal class ComponentEventWaiter
    {
        private readonly ConcurrentDictionary<string, ComponentMatchRequest> requests = new();
        private readonly DiscordFollowupMessageBuilder message;
        private readonly MenusConfiguration configuration;

        public ComponentEventWaiter(DiscordClient client, MenusConfiguration configuration)
        {
            client.ComponentInteractionCreated += Handle;
            this.configuration = configuration;
            message = new DiscordFollowupMessageBuilder().WithContent(configuration.ResponseMessage).AsEphemeral(true);
        }

        public async Task<(ComponentInteractionCreateEventArgs, MenuButtonDescriptor)?> WaitForMatchAsync(ComponentMatchRequest request)
        {
            requests[request.Id] = request;
            var result = await request.Tcs.Task.ConfigureAwait(false);
            requests.TryRemove(request.Id, out _);
            return result;
        }

        private async Task Handle(DiscordClient sender, ComponentInteractionCreateEventArgs args)
        {
            var id = args.Interaction.Data.CustomId;
            if (id.Length < MenusExtension.IdPrefix.Length
                || id[..MenusExtension.IdPrefix.Length] != MenusExtension.IdPrefix
                || id[MenusExtension.IdPrefix.Length] != MenusExtension.RegularMenuPrefix) return;

            var response = id[(MenusExtension.IdPrefix.Length + 1)..].ParseJson<MenuButtonDescriptor>();
            if (response is null || !requests.TryGetValue(response.MenuId, out var request))
            {
                if (configuration.ResponseBehaviour is ComponentResponseBehaviour.Ack or ComponentResponseBehaviour.Respond)
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
                if (configuration.ResponseBehaviour is ComponentResponseBehaviour.Respond)
                    await args.Interaction.CreateFollowupMessageAsync(message);
                return;
            }

            if (!request.IsMatch(args, response))
            {
                if (configuration.ResponseBehaviour is not ComponentResponseBehaviour.Respond) return;
                await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
                await args.Interaction.CreateFollowupMessageAsync(message);
                return;
            }

            request.Tcs.TrySetResult((args, response));
        }
    }
}