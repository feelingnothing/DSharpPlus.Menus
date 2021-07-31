using System;
using System.Threading.Tasks;
using DSharpPlus.Menus.Attributes;

namespace DSharpPlus.Menus.Entities
{
    public abstract class StaticMenu : MenuBase
    {
        public StaticMenu(string id, DiscordClient client) : base(client, id)
        {
            if (id.Length > 40) throw new ArgumentException("Id of the menu must 42 characters or less due to serialization behaviour");
            foreach (var (info, attribute) in CollectInteractionMethodsWithAttribute<StaticButtonAttribute>())
                Buttons.Add(new StaticMenuButton(attribute.Id, attribute.Style, info.CreateDelegate<Func<ButtonContext, Task>>(this),
                    attribute.Label, attribute.Location, attribute.Row, attribute.Disabled, attribute.Emoji));
        }

        public override Task StartAsync() => Task.FromException(new InvalidOperationException("Static menus cannot be started or stopped"));
        public override Task StopAsync(bool _ = false) => Task.FromException(new InvalidOperationException("Static menus cannot be started or stopped"));
    }
}