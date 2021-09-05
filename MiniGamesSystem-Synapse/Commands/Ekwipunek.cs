using System;
using CommandSystem;
using MiniGamesSystem.Hats;
using RemoteAdmin;
using Synapse;
using Synapse.Api;
using Synapse.Command;

namespace MiniGamesSystem.Commands
{
    [CommandInformation(
        Name = "ekwipunek",
        Aliases = new string[] { "eq" },
        Description = "Ekwipunek MiniGames.",
        Platforms = new[] { Platform.ClientConsole },
        Usage = "eq"
        )]

    public class Ekwipunek : ISynapseCommand
    {
        public string listaczapek(Player ply)
        {
            return string.Join("\n", Handler.pInfoDict[ply.UserId].ListaCzapek);
        }

        internal static bool RemoveHat(HatPlayerComponent playerComponent)
        {
            if (playerComponent.item == null) return false;

            UnityEngine.Object.Destroy(playerComponent.item.gameObject);
            playerComponent.item = null;
            return true;
        }

        public CommandResult Execute(CommandContext context)
        {
            var arguments = context.Arguments;
            var ply = Server.Get.GetPlayer(context.Player.PlayerId);
            if (arguments.Count == 0)
            {
                bool hasData = Handler.pInfoDict.ContainsKey(ply.UserId);
                return new CommandResult
                {
                    Message =
                "\n=================== Ekwipunek ===================\n" +
                "<color=#EFC01A>Twoje Czapki:</color>\n" +
                $"{(hasData ? listaczapek(ply) : "[NIE MASZ ŻADNYCH CZAPEK]")}\n" +
                "---------------------------\n" +
                "<color=cyan>Aby wziąć jakąś czapkę wpisz: </color><color=yellow>.eq wez [nazwa czapki]</color>\n" +
                "<color=cyan>Aby zdjąć czapkę wpisz: </color><color=yellow>.eq odloz</color>",
                    State = CommandResultState.Ok
                };
            }
            else if (arguments.At(0) == "wez")
            {
                if (arguments.At(1) == "Coin")
                {
                    if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Coin"))
                    {
                        if (ply.RoleType != RoleType.None && ply.RoleType != RoleType.Spectator)
                        {
                            //ply.SpawnHat(new HatInfo(ItemType.Coin, new UnityEngine.Vector3(1, 1, 1), ply.Position));
                            Hats.Hats.SpawnHat(ply, new HatInfo(ItemType.Coin));
                            return new CommandResult
                            {
                                Message = "<color=green>Założyłeś czapkę!</color>",
                                State = CommandResultState.Ok
                            };
                        }
                    }
                    else
                    {
                        return new CommandResult
                        {
                            Message = "<color=red>Nie masz takiej czapki w ekwipunku!</color>",
                            State = CommandResultState.Error
                        };
                    }
                }
                else if (arguments.At(1) == "Piłka")
                {
                    if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Piłka"))
                    {
                        if (ply.RoleType != RoleType.None && ply.RoleType != RoleType.Spectator)
                        {
                            ply.SpawnHat(new HatInfo(ItemType.SCP018, new UnityEngine.Vector3(1, 1, 1), ply.Position));
                            return new CommandResult
                            {
                                Message = "<color=green>Założyłeś czapkę!</color>",
                                State = CommandResultState.Ok
                            };
                        }
                    }
                    else
                    {
                        return new CommandResult
                        {
                            Message = "<color=red>Nie masz takiej czapki w ekwipunku!</color>",
                            State = CommandResultState.Error
                        };
                    }
                }
                else if (arguments.At(1) == "Cola")
                {
                    if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Cola"))
                    {
                        if (ply.RoleType != RoleType.None && ply.RoleType != RoleType.Spectator)
                        {
                            ply.SpawnHat(new HatInfo(ItemType.SCP207, new UnityEngine.Vector3(1, 1, 1), ply.Position));
                            return new CommandResult
                            {
                                Message = "<color=green>Założyłeś czapkę!</color>",
                                State = CommandResultState.Ok
                            };
                        }
                    }
                    else
                    {
                        return new CommandResult
                        {
                            Message = "<color=red>Nie masz takiej czapki w ekwipunku!</color>",
                            State = CommandResultState.Error
                        };
                    }
                }
                else if (arguments.At(1) == "Beret")
                {
                    if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Beret"))
                    {
                        if (ply.RoleType != RoleType.None && ply.RoleType != RoleType.Spectator)
                        {
                            ply.SpawnHat(new HatInfo(ItemType.SCP268, new UnityEngine.Vector3(1, 1, 1), ply.Position));
                            return new CommandResult
                            {
                                Message = "<color=green>Założyłeś czapkę!</color>",
                                State = CommandResultState.Ok
                            };
                        }
                    }
                    else
                    {
                        return new CommandResult
                        {
                            Message = "<color=red>Nie masz takiej czapki w ekwipunku!</color>",
                            State = CommandResultState.Error
                        };
                    }
                }
                else if (arguments.At(1) == "Ser")
                {
                    if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Ser"))
                    {
                        if (ply.RoleType != RoleType.None && ply.RoleType != RoleType.Spectator)
                        {
                            ply.SpawnHat(new HatInfo(ItemType.KeycardScientist, new UnityEngine.Vector3(2, 3, 2), ply.Position));
                            return new CommandResult
                            {
                                Message = "<color=green>Założyłeś czapkę!</color>",
                                State = CommandResultState.Ok
                            };
                        }
                    }
                    else
                    {
                        return new CommandResult
                        {
                            Message = "<color=red>Nie masz takiej czapki w ekwipunku!</color>",
                            State = CommandResultState.Error
                        };
                    }
                }
                else
                {
                    return new CommandResult
                    {
                        Message = "<color=red>Nie masz takiej czapki!</color>",
                        State = CommandResultState.Error
                    };
                }
            }
            else if (arguments.At(0) == "odloz")
            {
                HatPlayerComponent playerComponent;
                if (!ply.gameObject.TryGetComponent(out playerComponent))
                {
                    playerComponent = ply.gameObject.AddComponent<HatPlayerComponent>();
                }
                RemoveHat(playerComponent);
                return new CommandResult
                {
                    Message = "<color=green>Odłożyłeś czapkę!</color>",
                    State = CommandResultState.Ok
                };
            }
            return new CommandResult
            {
                Message = "",
                State = CommandResultState.Ok
            };
        }
    }
}
