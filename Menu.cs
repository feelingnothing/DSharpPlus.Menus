﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.Menus.Attributes;
using Microsoft.VisualBasic;

namespace DSharpPlus.Menus
{
    internal class Button
    {
        public Button(Guid id, ButtonStyle style, Func<DiscordInteraction, Task> callable, string content, bool disabled = false, DiscordComponentEmoji? emoji = null)
        {
            Id = id;
            Style = style;
            Callable = callable;
            Content = content;
            Disabled = disabled;
            Emoji = emoji;
        }

        public Guid Id { get; }
        public ButtonStyle Style { get; }
        public Func<DiscordInteraction, Task> Callable { get; }
        public string Content { get; }
        public bool Disabled { get; }
        public DiscordComponentEmoji? Emoji { get; }
    }

    public abstract class Menu
    {
        private readonly Guid id = Guid.NewGuid();
        private readonly string prefix;
        internal readonly List<Button> Buttons = new();

        protected Menu(DiscordClient client)
        {
            var ext = client.GetMenus();
            prefix = ext.Configuration.ComponentPrefix;
            foreach (var method in GetType().GetMethods())
            {
                if (!method.IsPublic || method.IsStatic || method.IsAbstract) continue;
                var parameters = method.GetParameters();
                if (parameters.Length is > 1 or 0) continue;
                var parameter = parameters.First();
                if (parameter.ParameterType != typeof(DiscordInteraction) || method.ReturnType != typeof(Task)) continue;
                var attr = method.GetCustomAttribute<ButtonAttribute>(true);
                if (attr is null) continue;
                Buttons.Add(new Button(attr.Id, attr.Style, method.CreateDelegate<Func<DiscordInteraction, Task>>(this), attr.Label, attr.Disabled));
            }
        }

        public virtual Task StartAsync()
        {
            MenusExtension.PendingMenus[id] = this;
            return Task.CompletedTask;
        }

        public DiscordMessageBuilder Serialize() => new DiscordMessageBuilder().AddComponents(
            Buttons.Select(b => new DiscordButtonComponent(b.Style, $"{id:B}{b.Id:B}", b.Content, b.Disabled, b.Emoji)));

        public virtual Task StopAsync()
        {
            if (!MenusExtension.PendingMenus.TryRemove(id, out _)) throw new InvalidOperationException("This menu is already stopped or has not started yet");
            return Task.CompletedTask;
        }
    }
}