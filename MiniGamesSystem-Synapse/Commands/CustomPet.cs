using System;
using CommandSystem;
using MiniGamesSystem.Pets;
using RemoteAdmin;
using Synapse;
using Synapse.Api;
using Synapse.Command;

namespace MiniGamesSystem.Commands
{
    [CommandInformation(
    Name = "custompet",
    Description = "CustomPet MiniGames.",
    Platforms = new[] { Platform.ClientConsole },
    Usage = "custompet"
    )]
    public class CustomPet : ISynapseCommand
    {

        public CommandResult Execute(CommandContext context)
        {
            var result = new CommandResult();
            var arguments = context.Arguments;
            var ply = Server.Get.GetPlayer(context.Player.PlayerId);
            if (Handler.pInfoDict[ply.UserId].ListaPetow.Contains(PetType.custom))
            {
                if (arguments.At(0) == null)
                {
                    result.Message =
                "\n=================== CUSTOM PET MENU <color=#EFC01A>(BARDZO WCZESNA BETA)</color> ===================\n" +
                "<color=#EFC01A>Dostępne Komendy:</color>\n" +
                ".custompet nazwa [nazwa peta] - ustawia nazwę peta\n" +
                ".custompet klasa [klasa peta np. SCP173] - zmienia klasę peta\n" +
                ".custompet item [nazwa itemu] - ustawia jaki item ma trzymać pet\n" +
                ".custompet rozmiar [x y z] - ustawia rozmiar peta\n" +
                "---------------------------\n" +
                "<color=#EFC01A>Aktualne Dane Peta:</color>\n" +
                "Nazwa:\n" +
                "Klasa:\n" +
                "Item\n" +
                "Rozmiar\n";
                    result.State = CommandResultState.Ok;
                    return result;
                }
            }
            return result;
        }
    }
}
