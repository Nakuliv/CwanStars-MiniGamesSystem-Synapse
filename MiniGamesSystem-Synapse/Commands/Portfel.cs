using System;
using CommandSystem;
using RemoteAdmin;
using Synapse;
using Synapse.Api;
using Synapse.Command;

namespace MiniGamesSystem.Commands
{
    [CommandInformation(
    Name = "portfel",
    Description = "Portfel MiniGames.",
    Platforms = new[] { Platform.ClientConsole },
    Usage = "portfel"
    )]
    public class Portfel : ISynapseCommand
    {

        public CommandResult Execute(CommandContext context)
        {
            var result = new CommandResult();
            var arguments = context.Arguments;
            var ply = Server.Get.GetPlayer(context.Player.PlayerId);

            Player player = arguments.Count == 0 ? ply : Server.Get.GetPlayer(arguments.At(0));
            string nick;
            bool hasData = Handler.pInfoDict.ContainsKey(player.UserId);
            if (player != null) nick = player.NickName;
            else nick = hasData ? Handler.pInfoDict[ply.UserId].Coins.ToString() : "[BRAK DANYCH]";
            result.Message =
                "\n=================== Portfel ===================\n" +
                $"Gracz: {nick} ({player.UserId})\n" +
                $"Coiny: {(hasData ? Handler.pInfoDict[player.UserId].Coins.ToString() : "[BRAK DANYCH]")}\n";
            result.State = CommandResultState.Ok;
            return result;
        }
    }
}
