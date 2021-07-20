# DSharpPlus.Menus

DSharpPlus interaction wrapper for menus allows you to create a classes with interactive buttons with callback  
For now this library exists just to comfort my requirements, but with more commits I'm making the more I see this extension used

Basic description of the extension that it just gives you interactivity with callbacks from components basically something that i am missing from official library, also gives you a static menus that can be registered at the beginning and will work even after restarting the bot because you are choosing ids for it 


To setup your project with menus download the package from nuget and add
```c#
client.UseMenus();
```
to your `DiscordClient` instance and you are ready to go  
Examples will be available soon.. maybe

This library is not production ready and should not be used because of massive changes and my bad coding skills
For a fact nuget versions are always tested and provided with stable core, if you really want to use this go to the nuget

### This is not official DSharpPlus extension and most probably will never be, use it on your own risk
