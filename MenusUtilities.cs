using System.Threading.Tasks;
using DSharpPlus.CommandsNext;

namespace DSharpPlus.Menus
{
    public static class MenusUtilities
    {
        public static async Task RespondWithMenuAsync(this CommandContext ctx, Menu menu, string? content = null)
        {
            await menu.StartAsync();
            var builder = menu.Serialize().WithContent(content);
            await ctx.RespondAsync(builder);
        }
    }
}