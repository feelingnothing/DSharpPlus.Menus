﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.Menus.Attributes;

namespace DSharpPlus.Menus
{
    internal class Button
    {
        public Button(ButtonStyle style, Func<DiscordInteraction, Task> callable, string content, int row = 0, bool disabled = false, DiscordComponentEmoji? emoji = null)
        {
            Style = style;
            Callable = callable;
            Content = content;
            Row = row;
            Disabled = disabled;
            Emoji = emoji;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public ButtonStyle Style { get; }
        public Func<DiscordInteraction, Task> Callable { get; }
        public string Content { get; }
        public int Row { get; }
        public bool Disabled { get; }
        public DiscordComponentEmoji? Emoji { get; }
    }

    public abstract class Menu
    {
        public DiscordClient Client { get; }
        
        private readonly Guid id = Guid.NewGuid();
        private readonly string prefix;
        internal readonly List<Button> Buttons = new();

        protected Menu(DiscordClient client)
        {
            Client = client;
            prefix = client.GetMenus().Configuration.ComponentPrefix;
            foreach (var method in GetType().GetMethods())
            {
                if (!method.IsPublic || method.IsStatic || method.IsAbstract) continue;
                var parameters = method.GetParameters();
                if (parameters.Length is > 1 or 0) continue;
                var parameter = parameters.First();
                if (parameter.ParameterType != typeof(DiscordInteraction) || method.ReturnType != typeof(Task)) continue;
                var attr = method.GetCustomAttribute<ButtonAttribute>(false);
                if (attr is null) continue;
                Buttons.Add(new Button(attr.Style, method.CreateDelegate<Func<DiscordInteraction, Task>>(this),
                    attr.Label, attr.Row, attr.Disabled, attr.Emoji));
            }
        }

        public virtual Task StartAsync()
        {
            MenusExtension.PendingMenus[id] = this;
            return Task.CompletedTask;
        }

        public IEnumerable<DiscordActionRowComponent> Serialize() => Buttons.GroupBy(b => b.Row).Select(g => new DiscordActionRowComponent(
            g.Select(b => new DiscordButtonComponent(b.Style, $"{prefix} {id} {b.Id}", b.Content, b.Disabled, b.Emoji))));

        public virtual Task StopAsync()
        {
            if (!MenusExtension.PendingMenus.TryRemove(id, out _)) throw new InvalidOperationException("This menu is already stopped or has not started yet");
            return Task.CompletedTask;
        }
    }
}