using System;
using System.Collections.Generic;
using CommandSystem;
using RemoteAdmin;
using Synapse;
using Synapse.Api;
using Synapse.Command;

namespace MiniGamesSystem.Commands
{
    [CommandInformation(
Name = "vote",
Aliases = new string[] { "vt" },
Description = "Vote MiniGames.",
Platforms = new[] { Platform.ClientConsole },
Usage = "vote [id eventu]"
)]
    public class Vote : ISynapseCommand
    {

        public static HashSet<string> vote = new HashSet<string>();

        public CommandResult Execute(CommandContext context)
        {
            var result = new CommandResult();
            var arguments = context.Arguments;
            Player ply = Server.Get.GetPlayer(context.Player.PlayerId);
            if (arguments.Count == 0)
            {
                if (!Round.Get.RoundIsActive)
                {
                    result.Message = "<color=red>musisz wpisać numer eventu!</color>";
                    return result;
                }
                else if (Round.Get.RoundIsActive)
                {
                    result.Message = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                    return result;
                }
            }
            else if (arguments.Count != 0)
            {
                switch (arguments.At(0).ToLower())
                {
                    case "1":
                        if (vote.Contains($"HAS{ply.UserId}"))
                        {
                            Handler.hideAndSeek--;
                            Handler.Deathmatch++;
                            vote.Remove($"HAS{ply.UserId}");
                            vote.Add($"DM{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na DeathMatch!";
                            return result;
                        }
                        else if (vote.Contains($"GW{ply.UserId}"))
                        {
                            Handler.GangWar--;
                            Handler.Deathmatch++;
                            vote.Remove($"GW{ply.UserId}");
                            vote.Add($"DM{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na DeathMatch!";
                            return result;
                        }
                        else if (vote.Contains($"DG{ply.UserId}"))
                        {
                            Handler.dgball--;
                            Handler.Deathmatch++;
                            vote.Remove($"DG{ply.UserId}");
                            vote.Add($"DM{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na DeathMatch!";
                            return result;
                        }
                        else if (vote.Contains($"PE{ply.UserId}"))
                        {
                            Handler.WarheadRun--;
                            Handler.Deathmatch++;
                            vote.Remove($"PE{ply.UserId}");
                            vote.Add($"DM{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na DeathMatch!";
                            return result;
                        }
                        else if (!Round.Get.RoundIsActive && !vote.Contains($"DM{ply.UserId}"))
                        {
                            Handler.Deathmatch++;
                            vote.Add($"DM{ply.UserId}");
                            result.Message = "Pomyślnie zagłosowano na DeathMatch!";
                            return result;
                        }
                        else if (vote.Contains($"DM{ply.UserId}"))
                        {
                            result.Message = "<color=red>Możesz zagłosować tylko raz!</color>";
                            return result;
                        }

                        else if (Round.Get.RoundIsActive)
                        {
                            result.Message = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                            return result;
                        }

                        break;
                    case "2":
                        if (vote.Contains($"DM{ply.UserId}"))
                        {
                            Handler.Deathmatch--;
                            Handler.GangWar++;
                            vote.Remove($"DM{ply.UserId}");
                            vote.Add($"GW{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na GangWar!";
                            return result;
                        }
                        else if (vote.Contains($"PE{ply.UserId}"))
                        {
                            Handler.WarheadRun--;
                            Handler.GangWar++;
                            vote.Remove($"PE{ply.UserId}");
                            vote.Add($"GW{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na GangWar!";
                            return result;
                        }
                        else if (vote.Contains($"DG{ply.UserId}"))
                        {
                            Handler.dgball--;
                            Handler.GangWar++;
                            vote.Remove($"DG{ply.UserId}");
                            vote.Add($"GW{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na GangWar!";
                            return result;
                        }
                        else if (vote.Contains($"HAS{ply.UserId}"))
                        {
                            Handler.hideAndSeek--;
                            Handler.GangWar++;
                            vote.Remove($"HAS{ply.UserId}");
                            vote.Add($"GW{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na GangWar!";
                        }
                        else if (!Round.Get.RoundIsActive && !vote.Contains($"GW{ply.UserId}"))
                        {
                            Handler.GangWar++;
                            vote.Add($"GW{ply.UserId}");
                            result.Message = "Pomyślnie zagłosowano na GangWar!";
                            return result;
                        }
                        else if (vote.Contains($"GW{ply.UserId}"))
                        {
                            result.Message = "<color=red>Możesz zagłosować tylko raz!</color>";
                            return result;
                        }

                        else if (Round.Get.RoundIsActive)
                        {
                            result.Message = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                            return result;
                        }
                        break;
                    case "3":
                        if (vote.Contains($"DM{ply.UserId}"))
                        {
                            Handler.Deathmatch--;
                            Handler.hideAndSeek++;
                            vote.Remove($"DM{ply.UserId}");
                            vote.Add($"HAS{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na HideAndSeek!";
                            return result;
                        }
                        else if (vote.Contains($"GW{ply.UserId}"))
                        {
                            Handler.GangWar--;
                            Handler.hideAndSeek++;
                            vote.Remove($"GW{ply.UserId}");
                            vote.Add($"HAS{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na HideAndSeek!";
                            return result;
                        }
                        else if (vote.Contains($"DG{ply.UserId}"))
                        {
                            Handler.dgball--;
                            Handler.hideAndSeek++;
                            vote.Remove($"DG{ply.UserId}");
                            vote.Add($"HAS{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na HideAndSeek!";
                            return result;
                        }
                        else if (vote.Contains($"PE{ply.UserId}"))
                        {
                            Handler.WarheadRun--;
                            Handler.hideAndSeek++;
                            vote.Remove($"PE{ply.UserId}");
                            vote.Add($"HAS{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na HideAndSeek!";
                            return result;
                        }
                        else if (!Round.Get.RoundIsActive && !vote.Contains($"HAS{ply.UserId}"))
                        {
                            Handler.hideAndSeek++;
                            vote.Add($"HAS{ply.UserId}");
                            result.Message = "Pomyślnie zagłosowano na HideAndSeek!";
                            return result;
                        }
                        else if (vote.Contains($"HAS{ply.UserId}"))
                        {
                            result.Message = "<color=red>Możesz zagłosować tylko raz!</color>";
                            return result;
                        }

                        else if (Round.Get.RoundIsActive)
                        {
                            result.Message = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                            return result;
                        }
                        break;
                    case "4":
                        if (vote.Contains($"DM{ply.UserId}"))
                        {
                            Handler.Deathmatch--;
                            Handler.WarheadRun++;
                            vote.Remove($"DM{ply.UserId}");
                            vote.Add($"PE{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na PeanutRun!";
                            return result;
                        }
                        else if (vote.Contains($"HAS{ply.UserId}"))
                        {
                            Handler.hideAndSeek--;
                            Handler.WarheadRun++;
                            vote.Remove($"HAS{ply.UserId}");
                            vote.Add($"PE{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na PeanutRun!";
                            return result;
                        }
                        else if (vote.Contains($"DG{ply.UserId}"))
                        {
                            Handler.dgball--;
                            Handler.WarheadRun++;
                            vote.Remove($"DG{ply.UserId}");
                            vote.Add($"PE{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na PeanutRun!";
                            return result;
                        }
                        else if (vote.Contains($"GW{ply.UserId}"))
                        {
                            Handler.GangWar--;
                            Handler.WarheadRun++;
                            vote.Remove($"GW{ply.UserId}");
                            vote.Add($"PE{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na PeanutRun!";
                            return result;
                        }
                        else if (!Round.Get.RoundIsActive && !vote.Contains($"PE{ply.UserId}"))
                        {
                            Handler.WarheadRun++;
                            vote.Add($"PE{ply.UserId}");
                            result.Message = "Pomyślnie zagłosowano na PeanutRun!";
                            return result;
                        }
                        else if (vote.Contains($"PE{ply.UserId}"))
                        {
                            result.Message = "<color=red>Możesz zagłosować tylko raz!</color>";
                            return result;
                        }

                        else if (Round.Get.RoundIsActive)
                        {
                            result.Message = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                            return result;
                        }
                        break;
                    case "5":
                        if (vote.Contains($"DM{ply.UserId}"))
                        {
                            Handler.Deathmatch--;
                            Handler.dgball++;
                            vote.Remove($"DM{ply.UserId}");
                            vote.Add($"DG{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na Dodgeball!";
                            return result;
                        }
                        else if (vote.Contains($"PE{ply.UserId}"))
                        {
                            Handler.WarheadRun--;
                            Handler.dgball++;
                            vote.Remove($"PE{ply.UserId}");
                            vote.Add($"DG{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na Dodgeball!";
                            return result;
                        }
                        else if (vote.Contains($"HAS{ply.UserId}"))
                        {
                            Handler.hideAndSeek--;
                            Handler.dgball++;
                            vote.Remove($"HAS{ply.UserId}");
                            vote.Add($"DG{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na Dodgeball!";
                            return result;
                        }
                        else if (vote.Contains($"GW{ply.UserId}"))
                        {
                            Handler.GangWar--;
                            Handler.dgball++;
                            vote.Remove($"GW{ply.UserId}");
                            vote.Add($"DG{ply.UserId}");
                            result.Message = "Pomyślnie zmieniono głos na Dodgeball!";
                            return result;
                        }
                        else if (!Round.Get.RoundIsActive && !vote.Contains($"DG{ply.UserId}"))
                        {
                            Handler.dgball++;
                            vote.Add($"DG{ply.UserId}");
                            result.Message = "Pomyślnie zagłosowano na Dodgeball!";
                            return result;
                        }
                        else if (vote.Contains($"DG{ply.UserId}"))
                        {
                            result.Message = "<color=red>Możesz zagłosować tylko raz!</color>";
                            return result;
                        }

                        else if (Round.Get.RoundIsActive)
                        {
                            result.Message = "<color=red>Nie możesz głosować w trakcie rundy!</color>";
                            return result;
                        }
                        break;
                }
            }
            result.Message = "<color=red>Musiałeś wipsać coś źle!</color>";
            return result;
        }
    }
}
