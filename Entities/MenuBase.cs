using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.Menus.Attributes;
using Newtonsoft.Json;

namespace DSharpPlus.Menus.Entities
{
    public enum MenuStatus
    {
        None = 0,
        Started = 1,
        Ended = 2,
    }

    public abstract class MenuBase
    {
        public DiscordClient Client { get; }
        public string Id { get; }
        public MenuStatus Status { get; protected internal set; } = MenuStatus.None;
        protected internal List<IMenuButton> Buttons { get; } = new();

        protected internal MenuBase(DiscordClient client, string id)
        {
            Id = id;
            Client = client;
        }

        protected internal IEnumerable<(MethodInfo, T)> CollectInteractionMethodsWithAttribute<T>() where T : BaseButtonAttribute
        {
            return GetType().GetMethods().Select<MethodInfo, (MethodInfo, T)?>(m =>
            {
                if (!m.IsPublic || m.IsStatic || m.IsAbstract) return null;
                var parameters = m.GetParameters();
                if (parameters.Length is > 1 or 0) return null;
                var parameter = parameters.First();
                if (parameter.ParameterType != typeof(DiscordInteraction) || m.ReturnType != typeof(Task)) return null;
                var attr = m.GetCustomAttribute<T>(false);
                if (attr is null) return null;
                return (m, attr);
            }).Where(t => t.HasValue).Select(t => t!.Value);
        }

        internal IEnumerable<DiscordActionRowComponent> Serialize() => Buttons.GroupBy(b => b.Row)
            .Select(g => new DiscordActionRowComponent(g.Select(b => new DiscordButtonComponent(b.Style,
                JsonConvert.SerializeObject(new Menus.MenuButton {MenuId = Id, ButtonId = b.Id}), b.Label, b.Disabled, b.Emoji))));

        public virtual Task<bool> CanBeExecuted(DiscordInteraction interaction) => Task.FromResult(true);
        public abstract Task StartAsync();
        public abstract Task StopAsync();
    }
}