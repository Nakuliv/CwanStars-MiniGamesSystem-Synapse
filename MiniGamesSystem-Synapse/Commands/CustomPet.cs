using System;
using System.Collections.Generic;
using System.IO;
using CommandSystem;
using MiniGamesSystem.Pets;
using Newtonsoft.Json;
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
            if (Handler.pInfoDict[ply.UserId].ListaPetow.Contains(PetType.Custom))
            {
                switch (arguments.At(0))
                {
                    case "help":
                        result.Message =
                    "\n=================== CUSTOM PET MENU <color=#EFC01A>(BETA)</color> ===================\n" +
                    "<color=#EFC01A>Dostępne Komendy:</color>\n" +
                    ".custompet nazwa [nazwa peta] - ustawia nazwę peta\n" +
                    ".custompet klasa [id klasy] - zmienia klasę peta\n" +
                    ".custompet item [id itemu] - ustawia jaki item ma trzymać pet\n" +
                    ".custompet rozmiar [x y z] - ustawia rozmiar peta\n" +
                    ".custompet spawn - spawnuje peta\n" +
                    "---------------------------\n" +
                    "<color=#EFC01A>Aktualne Dane Peta:</color>\n" +
                    $"Nazwa:{Handler.pInfoDict[ply.UserId].custompetName}\n" +
                    $"Klasa:{Handler.pInfoDict[ply.UserId].custompetClass}\n" +
                    $"Item: {Handler.pInfoDict[ply.UserId].custompetItem}\n" +
                    $"Rozmiar: {Handler.pInfoDict[ply.UserId].custompetSize}\n";
                        result.State = CommandResultState.Ok;
                        return result;
                    case "nazwa":
                        Handler.pInfoDict[ply.UserId].custompetName = arguments.At(1);
                        foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                        {
                            File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                        }
                        break;
                    case "klasa":
                        var klasa = Convert.ToInt32(arguments.At(1));
                        Handler.pInfoDict[ply.UserId].custompetClass = (RoleType)klasa;
                        foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                        {
                            File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                        }
                        break;
                    case "item":
                        var item = Convert.ToInt32(arguments.At(1));
                        Handler.pInfoDict[ply.UserId].custompetItem = (ItemType)item;
                        foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                        {
                            File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                        }
                        break;
                    case "rozmiar":
                        var x = Convert.ToInt32(arguments.At(1));
                        var y = Convert.ToInt32(arguments.At(2));
                        var z = Convert.ToInt32(arguments.At(3));
                        Handler.pInfoDict[ply.UserId].custompetSize = new UnityEngine.Vector3(x, y, z);
                        foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                        {
                            File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                        }
                        break;
                    case "spawn":
                        if (Pet.SpawnPet(ply, Handler.pInfoDict[ply.UserId].custompetName, PetType.Custom, out var pet))
                        {
                            pet.Player.ClassManager.CurClass = Handler.pInfoDict[ply.UserId].custompetClass;
                            pet.Player.ItemInHand = new Synapse.Api.Items.SynapseItem(Handler.pInfoDict[ply.UserId].custompetItem);
                            pet.Player.Scale = Handler.pInfoDict[ply.UserId].custompetSize;
                        }
                        break;
                }
                result.Message = "Error: coś zjebałeś";
                result.State = CommandResultState.Error;
                return result;
            }
            result.Message = "Error: nie masz custom peta w eq.";
            result.State = CommandResultState.Error;
            return result;
        }
    }
}
