using System;
using System.Linq;
using CommandSystem;
using RemoteAdmin;
using Synapse;
using Synapse.Api;
using Synapse.Command;

namespace MiniGamesSystem.Commands
{
    [CommandInformation(
Name = "topka",
Aliases = new string[] { "top" },
Description = "Top MiniGames.",
Platforms = new[] { Platform.ClientConsole },
Usage = "top"
)]
    public class Top : ISynapseCommand
    {
        public CommandResult Execute(CommandContext context)
        {
            var result = new CommandResult();
            var arguments = context.Arguments;
            Player ply = Server.Get.GetPlayer(context.Player.PlayerId);

            string output;
            int num = 5;
            if (arguments.Count > 0 && int.TryParse(arguments.At(0), out int n)) num = n;
            if (num > 15)
            {
                result.Message = "<color=red>Leaderboards can be no larger than 15.</color>";
                result.State = CommandResultState.Error;
                return result;
            }
            if (Handler.pInfoDict.Count != 0)
            {
                output = $"\n========== Top {num} najbogatszych graczy: ==========\n";

                for (int i = 0; i < num; i++)
                {
                    if (Handler.pInfoDict.Count == i) break;
                    string userid = Handler.pInfoDict.ElementAt(i).Key;
                    PlayerInfo info = Handler.pInfoDict[userid];
                    output += $"{i + 1}) <color=#EFC01A>{info.nick}</color> ({userid}) | Coiny: {info.Coins}";
                    if (i != Handler.pInfoDict.Count - 1) output += "\n";
                    else break;
                }
                result.Message = output;
                result.State = CommandResultState.Ok;
                return result;
            }
            else
            {
                result.Message = "<color=red>Error: Brak danych graczy.</color>";
                result.State = CommandResultState.Error;
                return result;
            }
        }
    }
}
