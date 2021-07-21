using System;

namespace DSharpPlus.Menus
{
    public enum ComponentResponseBehaviour
    {
        Ignore,
        Ack,
        Respond
    }

    public enum MenuButtonCallbackBehaviour
    {
        Synchronous,
        Asynchronous
    }

    public class MenusConfiguration
    {
        /// <summary>
        /// Controls the way a component interaction handled, if set to ignore the interaction could fail if button was not found
        /// If it set to Respond it will respond to every not found button with <inheritdoc cref="ResponseMessage"/>
        /// </summary>
        public ComponentResponseBehaviour ResponseBehaviour { internal get; set; } = ComponentResponseBehaviour.Ignore;

        /// <summary>
        /// If <inheritdoc cref="ResponseBehaviour"/> set to Respond, it will use this message as content
        /// </summary>
        public string ResponseMessage { internal get; set; } = "This was not meant for you";

        /// <summary>
        /// Default timespan to use for menu to timeout
        /// </summary>
        public TimeSpan DefaultMenuTimeout { internal get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Sets the behaviour of callable inside main menu loop
        /// Synchronous behaviour means that no other button would be processed if one of the buttons are executing at that time
        /// Asynchronous behaviour means that every button callback will be invoked in the background task;
        /// </summary>
        public MenuButtonCallbackBehaviour ButtonButtonCallback { internal get; set; } = MenuButtonCallbackBehaviour.Synchronous;
    }
}