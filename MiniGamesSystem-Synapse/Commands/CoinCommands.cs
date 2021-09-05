using System;
using CommandSystem;
using Synapse;
using Synapse.Command;

namespace MiniGamesSystem.Commands
{
    [CommandInformation(
        Name = "coin",
        Description = "Coiny do minigames.",
        Platforms = new[] { Platform.RemoteAdmin },
        Usage = "coin add [player id] / coin remove [player id]"
        )]

    public class CoinCommands : ISynapseCommand
    {

        public CommandResult Execute(CommandContext context)
        {
            var arguments = context.Arguments;
            var p = Server.Get.GetPlayer(context.Player.PlayerId);

            if (!p.HasPermission("MiniGamesSystem.addcoins"))
            {
                return new CommandResult
                {
                    Message = "<color=red>Nie masz uprawnień do tej komendy!</color>",
                    State = CommandResultState.NoPermission
                };
            }
            else
            {
                if (arguments.Count == 0 || arguments.Count == 0)
                {
                    return new CommandResult
                    {
                        Message = "<color=red>Musisz wpisać: coin add [id gracza] [ilość] lub coin remove [id gracza] [ilość]</color>",
                        State = CommandResultState.Error
                    };
                }
                else if (arguments.Count == 3)
                {
                    if (arguments.At(0) == "add")
                    {
                        int coiny = int.Parse(arguments.At(2));
                        Handler.pInfoDict[Server.Get.GetPlayer(arguments.At(1)).UserId].Coins = (Handler.pInfoDict[Server.Get.GetPlayer(arguments.At(1)).UserId].Coins + coiny);
                        return new CommandResult
                        {
                            Message = $"<color=green>Pomyślnie dodano {coiny} coinów graczowi {Server.Get.GetPlayer(arguments.At(1)).NickName}!</color>",
                            State = CommandResultState.Ok
                        };
                    }
                    else if (arguments.At(0) == "remove")
                    {
                        int coiny = int.Parse(arguments.At(2));
                        Handler.pInfoDict[Server.Get.GetPlayer(arguments.At(1)).UserId].Coins = (Handler.pInfoDict[Server.Get.GetPlayer(arguments.At(1)).UserId].Coins - coiny);
                        return new CommandResult
                        {
                            Message = $"<color=green>Pomyślnie usunięto {coiny} coinów graczowi {Server.Get.GetPlayer(arguments.At(1)).NickName}!</color>",
                            State = CommandResultState.Ok
                        };
                    }
                }
            }
            return new CommandResult
            {
                Message = "<color=red>Musiałeś wpisać coś źle!</color>",
                State = CommandResultState.Error
            };
        }
    }
}
