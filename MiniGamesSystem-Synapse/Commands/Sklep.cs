using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using CommandSystem;
using MiniGamesSystem.Pets;
using Newtonsoft.Json;
using RemoteAdmin;
using Synapse;
using Synapse.Command;

namespace MiniGamesSystem.Commands
{
    [CommandInformation(
    Name = "Sklep",
    Description = "Sklep MiniGames.",
    Platforms = new[] { Platform.ClientConsole },
    Usage = "sklep"
    )]
    public class Sklep : ISynapseCommand
    {
        public CommandResult Execute(CommandContext context)
        {
            var arguments = context.Arguments;
            var result = new CommandResult();
            var ply = Server.Get.GetPlayer(context.Player.PlayerId);
            if (arguments.Count == 0)
            {
                bool hasData = Handler.pInfoDict.ContainsKey(ply.UserId);
                result.Message =
                    "\n=================== Sklep ===================\n" +
                    $"twoje Coiny: {(hasData ? Handler.pInfoDict[ply.UserId].Coins.ToString() : "[BRAK DANYCH]")}\n" +
                    "---------------------------\n" +
                    "<color=#EFC01A>Czapki:</color>\n" +
                    "Coin - <color=yellow>50</color> coinów\n" +
                    "Piłka - <color=yellow>100</color> Coinów\n" +
                    "Cola - <color=yellow>150</color> Coinów\n" +
                    "Beret - <color=yellow>250</color> Coinów\n" +
                    "Ser - <color=yellow>1000</color> Coinów\n" +
                    "---------------------------\n" +
                    "<color=#EFC01A>Pety (BETA):</color>\n" +
                    "Amogus - <color=yellow>450</color> coinów\n" +
                    "---------------------------\n" +
                    "<color=#EFC01A>Rangi:</color>\n" +
                    "VIP na miesiąc - <color=yellow>10000</color> Coinów\n" +
                    "Admin - <color=yellow>999999999</color> Coinów\n" +
                    "---------------------------\n" +
                    "<color=cyan>Aby kupić jakiś item, wpisz:</color> <color=yellow>.sklep kup [nazwa itemu]</color>";
                result.State = CommandResultState.Ok;
                return result;
            }
            else if (arguments.Count > 0)
            {
                if (arguments.At(0) == "kup")
                {
                    if (arguments.At(1) == "Amogus")
                    {
                        if (Handler.pInfoDict[ply.UserId].Coins > 449)
                        {

                            if (Handler.pInfoDict[ply.UserId].ListaPetow.Contains(PetType.amogus))
                            {
                                result.Message = "<color=red>Masz już tego peta!</color>";
                                result.State = CommandResultState.Error;
                                return result;
                            }
                            else
                            {
                                Handler.pInfoDict[ply.UserId].Coins = (Handler.pInfoDict[ply.UserId].Coins - 450);
                                Handler.pInfoDict[ply.UserId].ListaPetow.Add(PetType.amogus);
                                foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                                {
                                    File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                }
                                result.Message = "<color=green>Kupiłeś Amogus, wpisz .eq aby zobaczyć listę twoich czapek i petów, lub nałożyć czapkę!</color>";
                                result.State = CommandResultState.Ok;
                                return result;
                            }
                        }
                        else
                        {
                            result.Message = "<color=red>Nie stać cię na to!</color>";
                            result.State = CommandResultState.Error;
                            return result;
                        }
                    }
                    else if (arguments.At(1) == "Vip")
                    {
                        if (Handler.pInfoDict[ply.UserId].Coins > 9999)
                        {

                            if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Vip"))
                            {
                                result.Message = "<color=red>Masz już tę rangę!</color>";
                                result.State = CommandResultState.Error;
                                return result;
                            }
                            else
                            {
                                Handler.pInfoDict[ply.UserId].Coins = (Handler.pInfoDict[ply.UserId].Coins - 10000);
                                Handler.pInfoDict[ply.UserId].ListaCzapek.Add("Vip");
                                result.Message = "<color=green>Kupiłeś rangę VIP na miesiąc!</color>";
                                result.State = CommandResultState.Ok;
                                foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                                {
                                    File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                }

                                //webhook
                                string token = "https://canary.discord.com/api/webhooks/875161096959442996/rKIGIZ0L7O7PR76rd-wuGsB9mE6OPC-lCqGiZtVMJXF8ShigAzbOD9wxyZy-RhTiMgBg";
                                WebRequest wr = (HttpWebRequest)WebRequest.Create(token);
                                wr.ContentType = "application/json";
                                wr.Method = "POST";
                                using (var sw = new StreamWriter(wr.GetRequestStream()))
                                {
                                    string json = JsonConvert.SerializeObject(new
                                    {
                                        username = "CwanStars Ban",
                                        embeds = new[]
                                        {
                        new
                        {
                            description = $"Gracz {ply.NickName} (steamid: {ply.UserId}) kupił VIPa na miesiąc za 10000 coinów!",
                            title = "Nowy VIP!",
                                                              image = new {
                    url = "https://cdn.discordapp.com/attachments/844546533507727380/875720054921105418/nowyvip2.png"
      },
                            color = 65417
            }
                    }
                                    });

                                    sw.Write(json);
                                }

                                var response = (HttpWebResponse)wr.GetResponse();
                                return result;
                            }
                        }
                        else
                        {
                            result.Message = "<color=red>Nie stać cię na to!</color>";
                            result.State = CommandResultState.Error;
                            return result;
                        }
                    }
                    else if (arguments.At(1) == "Coin")
                    {
                        if (Handler.pInfoDict[ply.UserId].Coins > 49)
                        {
                            if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Coin"))
                            {
                                result.Message = "<color=red>Masz już tę czapkę!</color>";
                                result.State = CommandResultState.Error;
                                return result;
                            }
                            else
                            {
                                Handler.pInfoDict[ply.UserId].Coins = (Handler.pInfoDict[ply.UserId].Coins - 50);
                                Handler.pInfoDict[ply.UserId].ListaCzapek.Add("Coin");
                                foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                                {
                                    File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                }
                                result.Message = "<color=green>Kupiłeś Coin, wpisz .eq aby zobaczyć listę twoich czapek, lub nałożyć czapkę!</color>";
                                result.State = CommandResultState.Ok;
                                return result;
                            }
                        }
                        else
                        {
                            result.Message = "<color=red>Nie stać cię na to!</color>";
                            result.State = CommandResultState.Error;
                            return result;
                        }
                    }
                    else if (arguments.At(1) == "Piłka")
                    {
                        if (Handler.pInfoDict[ply.UserId].Coins > 99)
                        {
                            if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Piłka"))
                            {
                                result.Message = "<color=red>Masz już tę czapkę!</color>";
                                result.State = CommandResultState.Error;
                                return result;
                            }
                            else
                            {
                                Handler.pInfoDict[ply.UserId].Coins = (Handler.pInfoDict[ply.UserId].Coins - 100);
                                Handler.pInfoDict[ply.UserId].ListaCzapek.Add("Piłka");
                                foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                                {
                                    File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                }
                                result.Message = "<color=green>Kupiłeś Piłkę, wpisz .eq aby zobaczyć listę twoich czapek, lub nałożyć czapkę!</color>";
                                result.State = CommandResultState.Ok;
                                return result;
                            }
                        }
                        else
                        {
                            result.Message = "<color=red>Nie stać cię na to!</color>";
                            result.State = CommandResultState.Error;
                            return result;
                        }
                    }
                    else if (arguments.At(1) == "Cola")
                    {
                        if (Handler.pInfoDict[ply.UserId].Coins > 149)
                        {
                            if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Cola"))
                            {
                                result.Message = "<color=red>Masz już tę czapkę!</color>";
                                result.State = CommandResultState.Error;
                                return result;
                            }
                            else
                            {
                                Handler.pInfoDict[ply.UserId].Coins = (Handler.pInfoDict[ply.UserId].Coins - 150);
                                Handler.pInfoDict[ply.UserId].ListaCzapek.Add("Cola");
                                foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                                {
                                    File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                }
                                result.Message = "<color=green>Kupiłeś Colę, wpisz .eq aby zobaczyć listę twoich czapek, lub nałożyć czapkę!</color>";
                                result.State = CommandResultState.Ok;
                                return result;
                            }
                        }
                        else
                        {
                            result.Message = "<color=red>Nie stać cię na to!</color>";
                            result.State = CommandResultState.Error;
                            return result;
                        }
                    }
                    else if (arguments.At(1) == "Beret")
                    {
                        if (Handler.pInfoDict[ply.UserId].Coins > 249)
                        {
                            if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Beret"))
                            {
                                result.Message = "<color=red>Masz już tę czapkę!</color>";
                                result.State = CommandResultState.Error;
                                return result;
                            }
                            else
                            {
                                Handler.pInfoDict[ply.UserId].Coins = (Handler.pInfoDict[ply.UserId].Coins - 250);
                                Handler.pInfoDict[ply.UserId].ListaCzapek.Add("Beret");
                                foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                                {
                                    File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                }
                                result.Message = "<color=green>Kupiłeś Beret, wpisz .eq aby zobaczyć listę twoich czapek, lub nałożyć czapkę!</color>";
                                result.State = CommandResultState.Ok;
                                return result;
                            }
                        }
                        else
                        {
                            result.Message = "<color=red>Nie stać cię na to!</color>";
                            result.State = CommandResultState.Error;
                            return result;
                        }
                    }
                    else if (arguments.At(1) == "Ser")
                    {
                        if (Handler.pInfoDict[ply.UserId].Coins > 999)
                        {
                            if (Handler.pInfoDict[ply.UserId].ListaCzapek.Contains("Ser"))
                            {
                                result.Message = "<color=red>Masz już tę czapkę!</color>";
                                result.State = CommandResultState.Error;
                                return result;
                            }
                            else
                            {
                                Handler.pInfoDict[ply.UserId].Coins = (Handler.pInfoDict[ply.UserId].Coins - 1000);
                                Handler.pInfoDict[ply.UserId].ListaCzapek.Add("Ser");
                                foreach (KeyValuePair<string, PlayerInfo> info in Handler.pInfoDict)
                                {
                                    File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
                                }
                                result.Message = "<color=green>Kupiłeś Ser, wpisz .eq aby zobaczyć listę twoich czapek, lub nałożyć czapkę!</color>";
                                result.State = CommandResultState.Ok;
                                return result;
                            }
                        }
                        else
                        {
                            result.Message = "<color=red>Nie stać cię na to!</color>";
                            result.State = CommandResultState.Error;
                            return result;
                        }
                    }
                    else
                    {
                        result.Message = "<color=red>Taki item nie istnieje, sprawdź czy wpisałeś nazwę itemu poprawnie!</color>";
                        result.State = CommandResultState.Error;
                        return result;
                    }
                }
            }
            result.Message = "";
            result.State = CommandResultState.Ok;
            return result;
        }
    }
}
