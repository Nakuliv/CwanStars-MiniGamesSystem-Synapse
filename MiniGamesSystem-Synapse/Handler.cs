﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEC;
using UnityEngine;
using YamlDotNet.Serialization;
using System.IO;
using Newtonsoft.Json;
using Respawning;
using Respawning.NamingRules;
using Synapse.Api.Events.SynapseEventArguments;
using MiniGamesSystem.Hats;
using Synapse.Api;
using Synapse.Api.Enum;
using Synapse;
using Synapse.Api.Items;

namespace MiniGamesSystem
{
    public class Handler
    {
        public static string EventMsg = "[<color=blue>Tryb</color>]";
        public MiniGamesSystem.ObjectType ObjectType { get; set; } = MiniGamesSystem.ObjectType.WorkStation;
        static System.Random rnd = new System.Random();
        StringBuilder message = new StringBuilder();
        public List<Vector3> PossibleSpawnsPos = new List<Vector3>();
        Vector3 ChoosedSpawnPos;
        public List<CoroutineHandle> coroutines = new List<CoroutineHandle>();
        public static Dictionary<string, PlayerInfo> pInfoDict = new Dictionary<string, PlayerInfo>();
        private List<DoorType> EscapePrimary = new List<DoorType>() { DoorType.Escape_Primary };
        private List<DoorType> SurfaceGate = new List<DoorType>() { DoorType.Surface_Gate };
        public static HashSet<string> props = new HashSet<string>();
        [YamlIgnore]
        public GameObject orginalPrefab;

        public static int Deathmatch = 0;
        public static int GangWar = 0;
        public static int hideAndSeek = 0;
        public static int WarheadRun = 0;
        public static int dgball = 0;

        public static string AktualnyEvent = "";

        internal static bool RemoveHat(HatPlayerComponent playerComponent)
        {
            if (playerComponent.item == null) return false;

            UnityEngine.Object.Destroy(playerComponent.item.gameObject);
            playerComponent.item = null;
            return true;
        }
        public string listaczapek(Player ply)
        {
            return string.Join("\n", pInfoDict[ply.UserId].ListaCzapek);
        }

        public void OnSpawningItems(Synapse.Api.Events.SynapseEventArguments.PlayerItemInteractEventArgs ev)
        {
            ev.Allow = false;
        }

        public void OnWTP()
        {
            SpawnManager();
            GameObject.Find("StartRound").transform.localScale = new Vector3(0, 0, 0);
            pInfoDict.Clear();
            foreach (FileInfo file in new DirectoryInfo(MiniGamesSystem.DataPath).GetFiles())
            {
                PlayerInfo info = JsonConvert.DeserializeObject<PlayerInfo>(File.ReadAllText(file.FullName));
                if (info.Coins == 0)
                {
                    File.Delete(file.FullName);
                    continue;
                }
                string userid = file.Name.Replace(".json", "");
                if (!pInfoDict.ContainsKey(userid) && !pInfoDict.ContainsValue(info))
                    pInfoDict.Add(userid, info);
            }
            pInfoDict = pInfoDict.OrderByDescending(x => x.Value.Coins).ToDictionary(x => x.Key, x => x.Value);

            var Unit = new SyncUnit();
            Unit.UnitName =
                "<color=#EFC01A>Witaj na</color> <color=green>Cwan</color><color=yellow>Stars</color>";
            Unit.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            var Unit1 = new SyncUnit();
            Unit1.UnitName = $"<color=#666699>Wejdź na naszego Discorda!</color>";
            Unit1.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            var Unit2 = new SyncUnit();
            Unit2.UnitName =
                "<color=#EFC01A>== Dostępne komendy ==</color>";
            Unit2.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            var Unit3 = new SyncUnit();
            Unit3.UnitName = $"<color=white>-</color> <color=#FAFF86>.vote</color>";
            Unit3.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            var Unit4 = new SyncUnit();
            Unit4.UnitName = $"<color=white>-</color> <color=#FAFF86>.sklep</color>";
            Unit4.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            var Unit5 = new SyncUnit();
            Unit5.UnitName = $"<color=white>-</color> <color=#FAFF86>.portfel</color>";
            Unit5.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            var Unit6 = new SyncUnit();
            Unit6.UnitName = $"<color=white>-</color> <color=#FAFF86>.eq</color>";
            Unit6.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            var Unit7 = new SyncUnit();
            Unit7.UnitName = $"<color=white>-</color> <color=#FAFF86>.top</color>";
            Unit7.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            var Space = new SyncUnit();
            Space.UnitName = $" ";
            Space.SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox;

            Timing.CallDelayed(0.001f, () => { RespawnManager.Singleton.NamingManager.AllUnitNames.Clear(); });
            Timing.CallDelayed(0.01f, () =>
            {
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Unit);
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Unit1);
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Space);
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Unit2);
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Unit3);
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Unit4);
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Unit5);
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Unit6);
                RespawnManager.Singleton.NamingManager.AllUnitNames.Add(Unit7);
            });
            foreach (CoroutineHandle coroutine in coroutines)
            {
                Timing.KillCoroutines(coroutine);
            }
            coroutines.Clear();

            coroutines.Add(Timing.RunCoroutine(LobbyTimer()));

            foreach (Door door in Map.Get.Doors)
            {
                if (EscapePrimary.Contains(door.DoorType))
                {
                    door.Locked = true;
                }

                if (SurfaceGate.Contains(door.DoorType))
                {
                    door.Locked = true;
                    door.Open = false;
                }

            }

        }

        public void OnWarheadDetonated()
        {
            if (AktualnyEvent == "WarheadRun")
            {
                Round.Get.RoundLock = false;
            }
        }

        /*public void OnWarheadCancel(StoppingEventArgs ev)
        {
            if (AktualnyEvent == "WarheadRun")
            {
                ev.IsAllowed = false;
            }
        }*/

        public void OnPlyDied(PlayerDeathEventArgs ev)
        {
            if (AktualnyEvent == "deathMatch")
            {
                if (pInfoDict.ContainsKey(ev.Killer.UserId))
                {
                    PlayerInfo info = pInfoDict[ev.Killer.UserId];
                    info.Coins++;

                    pInfoDict[ev.Killer.UserId] = info;
                }
            }
        }

        public void SpawnManager()
        {
            UnityEngine.Vector3 location = Map.Get.Rooms.First(x => x.Zone == ZoneType.Surface).Position;
            location.y = location.y + 1;

            PossibleSpawnsPos.Clear();

            PossibleSpawnsPos.Add(new Vector3(location.x, location.y, location.z + 10));

            PossibleSpawnsPos.ShuffleList();

            ChoosedSpawnPos = PossibleSpawnsPos[0];
        }

        public void OnRS()
        {
            if (AktualnyEvent == "WarheadRun")
            {
                Map.Get.Nuke.InsidePanel.Locked = true;
            }
                foreach (Player ply in Server.Get.Players)
            {
                ply.GodMode = false;
                ply.PlayerEffectsController.DisableEffect<CustomPlayerEffects.Scp207>();
                ply.SetRank("", "default");
                if (Extensions.hasTag) ply.RefreshTag();
                if (Extensions.isHidden) ply.GetComponent<ReferenceHub>().characterClassManager.CmdRequestHideTag();
            }
            Timing.CallDelayed(1.5f, () =>
            {
            if (Deathmatch > (GangWar + hideAndSeek + dgball + WarheadRun))
            {
                AktualnyEvent = "deathMatch";
                MiniGames.deathMatch();
            }
            else if (WarheadRun > (Deathmatch + hideAndSeek + GangWar + dgball))
            {
                AktualnyEvent = "WarheadRun";
                MiniGames.WarheadRunn();
            }
            else if (GangWar > (Deathmatch + hideAndSeek + WarheadRun + dgball))
            {
                AktualnyEvent = "WojnaGangow";
                MiniGames.WojnaGangow();
            }
            else if (hideAndSeek > (GangWar + Deathmatch + WarheadRun + dgball))
            {
                AktualnyEvent = "HideAndSeek";
                MiniGames.HideAndSeek();
            }
            else if (dgball > (GangWar + Deathmatch + WarheadRun + hideAndSeek))
            {
                AktualnyEvent = "DodgeBall";
                MiniGames.DgBall();
            }
            else
            {
                switch (rnd.Next(1, 6))
                {
                    case 1:
                        AktualnyEvent = "deathMatch";
                        MiniGames.deathMatch();
                        break;
                    case 2:
                        AktualnyEvent = "WojnaGangow";
                        MiniGames.WojnaGangow();
                        break;
                    case 3:
                        AktualnyEvent = "HideAndSeek";
                        MiniGames.HideAndSeek();
                        break;
                    case 4:
                        AktualnyEvent = "WarheadRun";
                        MiniGames.WarheadRunn();
                        break;
                    case 5:
                        AktualnyEvent = "DodgeBall";
                        MiniGames.DgBall();
                        break;
                }
                return;
            }
            Map.Get.SendBroadcast(5, $"{EventMsg} <b><color>{AktualnyEvent}</color></b>");
            });
        }

        public void OnRespawning(TeamRespawnEventArgs ev)
        {
            ev.Allow = false;
        }

        public void OnRndEnd()
        {
            if (AktualnyEvent == "WarheadRun")
            {
                if (Server.Get.GetPlayers(x => x.RoleType == RoleType.Scp173).Count() > 1)
                {
                    foreach (Player ply in Server.Get.GetPlayers(x => x.RoleType == RoleType.Scp173))
                    {
                        if (pInfoDict.ContainsKey(ply.UserId))
                        {
                            PlayerInfo info = pInfoDict[ply.UserId];
                            info.Coins = info.Coins + 1;

                            pInfoDict[ply.UserId] = info;
                        }

                        Map.Get.SendBroadcast(5, $"{ply.NickName} wygrali!");
                    }
                }
                if (Server.Get.GetPlayers(x => x.RoleType == RoleType.Scp173).Count() == 1)
                {
                    foreach (Player ply in Server.Get.GetPlayers(x => x.RoleType == RoleType.Scp173))
                    {
                        if (pInfoDict.ContainsKey(ply.UserId))
                        {
                            PlayerInfo info = pInfoDict[ply.UserId];
                            info.Coins = info.Coins + 1;

                            pInfoDict[ply.UserId] = info;
                        }

                        Map.Get.SendBroadcast(5, $"{ply.NickName} wygrał!");
                    }
                }
            }
            else if (AktualnyEvent == "deathMatch")
            {
                foreach (Player ply in Server.Get.GetPlayers(x => x.RoleType == RoleType.ClassD))
                {
                    if (pInfoDict.ContainsKey(ply.UserId))
                    {
                        PlayerInfo info = pInfoDict[ply.UserId];
                        info.Coins = info.Coins + 2;

                        pInfoDict[ply.UserId] = info;
                    }
                }
            }
            else if (AktualnyEvent == "WojnaGangow")
            {
                var team1 = Server.Get.GetPlayers(x => x.RoleType == RoleType.ClassD);
                var team2 = Server.Get.GetPlayers(x => x.RoleType == RoleType.Scientist);
                if (team1.Count() == 0 && team2.Count() > 0)
                {
                    foreach (Player ply in team2)
                    {
                        if (pInfoDict.ContainsKey(ply.UserId))
                        {
                            if (pInfoDict.ContainsKey(ply.UserId))
                            {
                                PlayerInfo info = pInfoDict[ply.UserId];
                                info.Coins++;

                                pInfoDict[ply.UserId] = info;
                            }
                        }
                    }
                    Map.Get.SendBroadcast(5, "<b><color=#FAFF86>Naukowcy</color> wygrali!</b>");
                }
                else if (team2.Count() == 0 && team1.Count() > 0)
                {
                    foreach (Player ply in team1)
                    {
                        if (pInfoDict.ContainsKey(ply.UserId))
                        {
                            PlayerInfo info = pInfoDict[ply.UserId];
                            info.Coins++;

                            pInfoDict[ply.UserId] = info;
                        }
                    }
                    Map.Get.SendBroadcast(5, "<b><color=#EE7600>Klasa-D</color> wygrała!</b>");
                }
            }
            else if (AktualnyEvent == "DodgeBall")
            {
                foreach (Player ply in Server.Get.GetPlayers(x => x.RoleType == RoleType.ClassD))
                {
                    if (pInfoDict.ContainsKey(ply.UserId))
                    {
                        PlayerInfo info = pInfoDict[ply.UserId];
                        info.Coins++;

                        pInfoDict[ply.UserId] = info;
                    }
                }
            }
        }
        public void OnRoundRestart()
        {
            Deathmatch = 0;
            GangWar = 0;
            hideAndSeek = 0;
            WarheadRun = 0;
            dgball = 0;
            Commands.Vote.vote.Clear();
            AktualnyEvent = "";

            foreach (CoroutineHandle coroutine in coroutines)
            {
                Timing.KillCoroutines(coroutine);
            }
            coroutines.Clear();

            Timing.KillCoroutines("dmCR");
            Timing.KillCoroutines("dmcheck");
            Timing.KillCoroutines("hascheck");
            Timing.KillCoroutines("dgballLoop");
            Timing.KillCoroutines();
            MiniGames.team1.Clear();

            foreach (KeyValuePair<string, PlayerInfo> info in pInfoDict)
            {
                File.WriteAllText(Path.Combine(MiniGamesSystem.DataPath, $"{info.Key}.json"), JsonConvert.SerializeObject(info.Value, Formatting.Indented));
            }
        }

        public void OnShooting(PlayerShootEventArgs ev)
        {
            foreach (Player ply in Server.Get.Players)
            {
                if (props.Contains(ply.UserId))
                {
                    if (ev.TargetPosition == ply.Position)
                    {
                        ply.Kill(DamageTypes.Com18);
                    }
                }
            }
        }

        public void OnPickingUp(PlayerPickUpItemEventArgs ev)
        {
            if (ev.Item.PickupBase.gameObject.TryGetComponent<HatItemComponent>(out var hat))
            {
                ev.Player.GiveTextHint("<color=red>Nie możesz podnieść czapki!</color>");
                ev.Allow = false;
            }
            /*if (props.Contains(ev.Player.UserId))
            {
                Item item = new Item(ev.Pickup.Type);
                
                item.Spawn(ev.Pickup.Position, default);

                ev.Player.IsInvisible = true;
                ev.IsAllowed = false;

                if (ev.Pickup.Type == ItemType.SCP018 || ev.Pickup.Type == ItemType.Medkit)
                {
                    ev.Player.Scale = new Vector3(0.01f, 0.01f, 0.01f);

                }
                else if (ev.Pickup.Type == ItemType.GunE11SR)
                {
                    ev.Player.Scale = new Vector3(0.1f, 0.1f, 0.1f);
                }
                Timing.RunCoroutine(TpProps(ev.Pickup));
            }*/
        }

        /*public IEnumerator<float> TpProps(Pickup type)
        {
            while (true)
            {
                foreach (Player ply in Player.List)
                {
                    if (props.Contains(ply.UserId))
                    {
                        type.Position = ply.Position;
                    }
                }
                yield return Timing.WaitForSeconds(0f);
            }
        }*/

        /*public IEnumerator<float> Czapki(Player ply, ItemType type)
        {
            SynapseItem item = new SynapseItem(type);
            
            Pickup Item = item.Spawn(ply.Position, default);
            while (true)
            {
                switch (ply.Role)
                {
                    case RoleType.Scp173:
                        Item.Position = new Vector3(0, .7f, -.05f) + ply.Position;
                        break;
                    case RoleType.Scp106:
                        Item.Position = new Vector3(0, .45f, .13f) + ply.Position;
                        break;
                    case RoleType.Scp096:
                        Item.Position = new Vector3(.15f, .45f, .225f) + ply.Position;
                        break;
                    case RoleType.Scp93953:
                        Item.Position = new Vector3(0, -.4f, 1.3f) + ply.Position;
                        break;
                    case RoleType.Scp93989:
                        Item.Position = new Vector3(0, -.3f, 1.3f) + ply.Position;
                        break;
                    case RoleType.Scp049:
                        Item.Position = new Vector3(0, .125f, -.05f) + ply.Position;
                        break;
                    case RoleType.None:
                        Item.Position = new Vector3(-1000, -1000, -1000) + ply.Position;
                        break;
                    case RoleType.Spectator:
                        Item.Position = new Vector3(-1000, -1000, -1000) + ply.Position;
                        break;
                    case RoleType.Scp0492:
                        Item.Position = new Vector3(0, 0f, -.06f) + ply.Position;
                        break;
                    default:
                        Item.Position = new Vector3(0, .15f, -.07f) + ply.Position;
                        break;
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
        }*/

        public void OnJoin(PlayerJoinEventArgs ev)
        {
            if (!File.Exists(Path.Combine(MiniGamesSystem.DataPath, $"{ev.Player.UserId}.json")))
            {
                pInfoDict.Add(ev.Player.UserId, new PlayerInfo(ev.Player.NickName));
            }

            if (!Round.Get.RoundIsActive)
            {
                Timing.CallDelayed(0.5f, () =>
                {
                    ev.Player.RoleType = RoleType.NtfCaptain;
                    ev.Player.GodMode = true;
                    ev.Player.RankName = "W lobby";
                    ev.Player.RankColor = "pumpkin";
                });

                Timing.CallDelayed(1f, () =>
                {
                    ev.Player.Inventory.Clear();
                    ev.Player.GiveEffect(Effect.Scp207, 4, 999f);
                    ev.Player.Position = ChoosedSpawnPos;
                });

            }
            else if (Round.Get.RoundIsActive)
            {
                ev.Player.SendBroadcast(5, $"<b><i>Aktualny Tryb: <color=green>{AktualnyEvent}</color></i></b>");
            }
        }

        private IEnumerator<float> LobbyTimer()
        {
            while (!Round.Get.RoundIsActive)
            {
                message.Clear();

                if (MiniGamesSystem.Config.HintVertPos != 0 && MiniGamesSystem.Config.HintVertPos < 0)
                {
                    for (int i = MiniGamesSystem.Config.HintVertPos; i < 0; i++)
                    {
                        message.Append("\n");
                    }
                }


                message.Append($"<size=25><B><color=#00fe0f>DeathMatch</color> [id: 1 | głosy: {Deathmatch}]  |  <color=#00fe0f>WojnaGangów</color> [id: 2 | głosy: {GangWar}]  |  <color=#00fe0f>HideAndSeek</color> [id: 3 | głosy: {hideAndSeek}]     <color=#00fe0f>WarheadRun</color> [id: 4 | głosy: {WarheadRun}]  |  <color=#00fe0f>Dodgeball</color> [id: 5 | głosy: {dgball}]</size><size=100><color=yellow>" + MiniGamesSystem.Config.TopMessage);

                short NetworkTimer = GameCore.RoundStart.singleton.NetworkTimer;

                switch (NetworkTimer)
                {
                    case -2: message.Replace("%seconds", MiniGamesSystem.Config.ServerIsPaused); break;

                    case -1: message.Replace("%seconds", MiniGamesSystem.Config.RoundIsBeingStarted); break;

                    case 1: message.Replace("%seconds", $"<color=green>{NetworkTimer}</color> {MiniGamesSystem.Config.OneSecondRemain}"); break;

                    case 0: message.Replace("%seconds", MiniGamesSystem.Config.RoundIsBeingStarted); break;

                    default: message.Replace("%seconds", $"<color=green>{NetworkTimer}</color> {MiniGamesSystem.Config.XSecondsRemains}"); break;
                }

                int NumOfPlayers = Server.Get.PlayersAmount;

                message.Append($"\n{MiniGamesSystem.Config.BottomMessage}");

                if (NumOfPlayers == 1) message.Replace("%players", $"<color=green>{NumOfPlayers}</color> {MiniGamesSystem.Config.OnePlayerConnected}");
                else message.Replace("%players", $"<color=green>{NumOfPlayers}</color> {MiniGamesSystem.Config.XPlayersConnected}");


                if (MiniGamesSystem.Config.HintVertPos != 0 && MiniGamesSystem.Config.HintVertPos > 0)
                {
                    for (int i = 0; i < MiniGamesSystem.Config.HintVertPos; i++)
                    {
                        message.Append("\n");
                    }
                }


                foreach (Player ply in Server.Get.Players)
                {
                    if (MiniGamesSystem.Config.UseHints) ply.GiveTextHint(message.ToString(), 1f);
                    else ply.SendBroadcast(1, message.ToString());
                }

                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}
