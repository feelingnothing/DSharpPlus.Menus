# DSharpPlus.Menus

[![Nuget](https://img.shields.io/nuget/vpre/DSharpPlus.Menus?style=flat-square)](https://www.nuget.org/packages/IDoEverything.DSharpPlus.SlashCommands)

A extension to make interaction easier.

#### This is not official DSharpPlus extension and most probably will never be, use it on your own risk

DSharpPlus interaction wrapper allows you to create a classes with multiple interactive buttons  
For now this library exists just to comfort my requirements, but with more commits I'm making - the more I see this extension used in everyone's projects, it is so easy to use and fits a in
a lot of projects

To setup your project with menus download the package from nuget and add

```c#
client.UseMenus();
```

to your `DiscordClient` instance and you are ready to go

# Examples

### Regular Menus

To use simple auto-generated menus use provided `Menus.Entities.Menu` class

```c#
class MyMenu : Menu
{
    public MyMenu(DiscordClient client) : base(client)

    // For this button to be registered it must have one of the button attributes,
    // have `ComponentInteractionCreateEventArgs` as first and only parameter and return `Task`
    [SuccessButton("It is a success button")]
    public async Task SuccessAsync(ComponentInteractionCreateEventArgs args)
    {
        await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
        // Do not forget to add menu to the edited message or buttons won't show up
        await args.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().AddMenu(this).WithContent($"Your name is {args.User.Username} and id {args.User.Id}!"));
    }
```

To see the list of the button attributes go to `\examples\` folder  
And when you want to use it start it with `StartAsync`, but do not forget to send it!

```c#
[Group("menu")]
class MyCommandModule : BaseCommandModule
{
    [GroupCommand]
    public async Task SendMenuAsync(CommandContext ctx)
    {
        var menu = new MyMenu(ctx.Client);
        await menu.StartAsync();
        await ctx.RespondAsync(new DiscordMessageBuilder().AddMenu(menu).WithContent("Here is your menu, sir."));
    }
}
```

### Static Menus

They are very similar with regular ones but you can send multiple copies of one menu and each of this copy will be called in a single menu and the most notable thing about static menus that
they will work even after bot restart  
To start using it you need first of all create a static menu class with `Menus.Intities.StaticMenu`

#### Important note: Maximum length of the custom id in the static menus are 42 characters or less!!

```c#
class MyStaticMenu : StaticMenu
{
    // Important note that maximum length of the menu id and button id is 42 or less!
    public MyStaticMenu(DiscordClient client) : base("maximumLengthOfTheCustomIdIs42Characters", clisnt)
    {
    }
    
    // Static menus have their own buttons attributes, do not use regular ones, menu would not recognize them
    [StaticSecondaryButton("notForgetAboutTheLimitationOf42Characters", "Click to create a menu only for you")]
    public async Task CreateEphemeralMenuAsync(ComponentInteractionCreateEventArgs args)
    {
        await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
        var menu = new MyMenu(Client);
        // Start then send!
        await menu.StartAsync();
        await args.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder()
                    .AddMenu(this).AsEphemeral(true).WithContent("A menu only for you!"));
    }
    
}
```

Then you need to register this menu in your startup function

```c#
private static ServiceProvider ConfigureServices()
{
    return new ServiceCollection()
        .AddSingleton(_ => new DiscordClient(new DiscordConfiguration
        {
            Token = %YOUR_TOKEN%,
            TokenType = TokenType.Bot
        }))
        .AddSingleton(s => 
        {
            var client = s.GetRequiredService<DiscordClient>();
            return client.UseCommandsNext(new CommandsNextConfiguration
            {
                Services = s,
                StringPrefixes = new [] {".", "!", "~"}
            });
        })
        .AddSingleton(s =>
        {
            var client = s.GetRequiredService<DiscordClient>();
            return client.UseMenus();
        })
        .BuildServiceProvider(true);
}

private static async Task Main()
{
    await using var services = ConfigureServices();
    var client = services.GetRequiredService<DiscordClient>();
    var commands = services.GetRequiredService<CommandsNextExtension>();
    var menus = services.GetRequiredService<MenusExtension>();
    
    commands.RegisterCommands<MyCommandModule>();
    // Register static menu, also you can provide more parameters to constructor, there is no limit
    menus.RegisterStaticMenu(() => new MyStaticMenu(client));

    await client.ConnectAsync();
    await Task.Delay(-1);
}
```

Basic client setup and menu registration at Main function, now your menu is registered and already can accept button clicks, but there is not a single menu in your server, now we need to
send it  
Lets take previous command module and rewrite it a bit

```c#
[Group("menu")]
class MyCommandModule : BaseCommandModule
{
    // Our old command, lets leave it here
    [GroupCommand]
    public async Task SendMenuAsync(CommandContext ctx)
    {
        var menu = new MyMenu(ctx.Client);
        await menu.StartAsync();
        await ctx.RespondAsync(new DiscordMessageBuilder().AddMenu(menu).WithContent("Here is your menu, sir."));
    }
    
    // We really do not want users to create a static menu instance whenever they want
    // I can think of many ways users can interact with it, but the best way for me is closed channel with only this static menu 
    [Command("static"), RequireOwner]
    public async Task SendStaticMenuAsync(CommandContext ctx)
    {
        // If menu is not found it will throw `ArgumentException`
        var menu = ctx.Client.GetStaticMenu<MyStaticMenu>();
        // Important: We do not need to start it, it is running already if you registered it
        await ctx.ResponsdAsync(new DiscordMessageBuilder().WithContent("You can edit it anyway you want").AddMenu(menu));
    }
}
```

Now we can send a static menu that will work even after bot restarts, this would allow you to create a ephemeral menus only visible by person clicked it.

##### This is not official DSharpPlus extension and most probably will never be, use it on your own risk
