﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Menus.Attributes;

namespace DSharpPlus.Menus.Entities
{
    internal class StaticMenuButton : IMenuButton
    {
        public StaticMenuButton(string id, ButtonStyle style, Func<ComponentInteractionCreateEventArgs, Task> callable, string label, ButtonPosition location = ButtonPosition.First,
            ButtonPosition row = ButtonPosition.First, bool disabled = false, DiscordComponentEmoji? emoji = null)
        {
            Id = id;
            Style = style;
            Callable = callable;
            Label = label;
            Location = location;
            Row = row;
            Disabled = disabled;
            Emoji = emoji;
        }

        public string Id { get; }
        public ButtonStyle Style { get; }
        public Func<ComponentInteractionCreateEventArgs, Task> Callable { get; }
        public string Label { get; set; }
        public ButtonPosition Location { get; }
        public ButtonPosition Row { get; }
        public bool Disabled { get; set; }
        public DiscordComponentEmoji? Emoji { get; set; }
    }

    public abstract class StaticMenu : MenuBase
    {
        public StaticMenu(string id, DiscordClient client) : base(client, id)
        {
            if (id.Length > 40) throw new ArgumentException("Id of the menu must 42 characters or less due to serialization behaviour");
            Buttons = CollectInteractionMethodsWithAttribute<StaticButtonAttribute>().ToList().Select(((MethodInfo i, StaticButtonAttribute a) t) => new StaticMenuButton(t.a.Id, t.a.Style,
                t.i.CreateDelegate<Func<ComponentInteractionCreateEventArgs, Task>>(this), t.a.Label, t.a.Location, t.a.Row, t.a.Disabled, t.a.Emoji)).ToList();
        }

        public override Task StartAsync() => Task.FromException(new InvalidOperationException("Static menus cannot be started or stopped"));
        public override Task StopAsync(bool _ = false) => Task.FromException(new InvalidOperationException("Static menus cannot be started or stopped"));
    }
}