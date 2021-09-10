using System.Collections.Generic;
using System.Linq;
using MEC;
using UnityEngine;
using Synapse.Api.Enum;
using Synapse.Api;
using Synapse;
using MapGeneration;
using GameCore;

namespace MiniGamesSystem
{
    public class MiniGames
    {
        static System.Random rnd = new System.Random();
        public static Dictionary<string, PlayerInfo> pInfoDict = new Dictionary<string, PlayerInfo>();
        public static string EventMsg = "[<color=blue>Tryb</color>]";
        public static List<DoorType> lockedCheckpointLcz = new List<DoorType>() { DoorType.Checkpoint_LCZ_A, DoorType.Checkpoint_LCZ_B };
        public static List<DoorType> EscapePrimary = new List<DoorType>() { DoorType.Escape_Primary };
        public static List<DoorType> SurfaceGate = new List<DoorType>() { DoorType.Surface_Gate };
        public static List<DoorType> GangWarDoors = new List<DoorType>() { DoorType.Checkpoint_EZ_HCZ, DoorType.Gate_A, DoorType.Gate_B };
        public static List<string> team1 = new List<string>();
        public CoroutineHandle Coroutine { get; set; }

        //DeathMatch
        public static void deathMatch()
        {
            Round.Get.RoundLock = true;

            foreach (Door door in Map.Get.Doors)
            {
                if (door.DoorType == DoorType.Checkpoint_LCZ_A || door.DoorType == DoorType.Checkpoint_LCZ_B)
                {
                    door.Locked = true;
                }
            }
            foreach (Player player in Server.Get.Players)
            {
                player.RoleType = RoleType.ClassD;
                Vector3 spawnPos = Extensions.GetRandomSpawnPoint(RoleType.Scientist);

                Timing.CallDelayed(0.5f, () => player.Position = spawnPos);
            }

            Map.Get.SendBroadcast(5, "<b>Za 30 sekund wszyscy dostaną bronie i leczenie</b>");

            Timing.CallDelayed(25, () =>
            {
                Map.Get.SendBroadcast(1, "5");
                Map.Get.SendBroadcast(1, "4");
                Map.Get.SendBroadcast(1, "3");
                Map.Get.SendBroadcast(1, "2");
                Map.Get.SendBroadcast(1, "1");
            });
            Timing.CallDelayed(30, () =>
            {
                foreach (var ply in Server.Get.Players)
                    ply.GiveTextHint("Otrzymałeś broń i leczenie, sprawdź ekwipunek!", 5);
                foreach (Player player in Server.Get.Players)
                {
                    if (player.RoleType == RoleType.ClassD)
                    {
                        player.Inventory.Clear();
                        player.Inventory.AddItem(ItemType.GunCOM18);
                        player.Inventory.AddItem(ItemType.Adrenaline);
                        player.Inventory.AddItem(ItemType.Medkit);
                        player.AmmoBox[AmmoType.Ammo9x19] = 200;
                        player.AmmoBox[AmmoType.Ammo44cal] = 200;
                        player.AmmoBox[AmmoType.Ammo12gauge] = 200;
                    }
                }
            });


                Timing.CallDelayed(300, () =>
            {
                    Map.Get.SendBroadcast(1, "Checkpointy zostały otwarte!");

                    foreach (Door door in Map.Get.Doors)
                    {
                        if (door.DoorType == DoorType.Checkpoint_LCZ_A || door.DoorType == DoorType.Checkpoint_LCZ_B)
                        {
                            door.Locked = false;
                            door.TryBreakDoor();
                        }
                    }
            });

            foreach (Synapse.Api.Items.SynapseItem item in Map.Get.Items)
                item.Destroy();

            Timing.RunCoroutine(DeathMatchCheck(), "dmcheck");
        }
        private static IEnumerator<float> DeathMatchCheck()
        {
            while (true)
            {
                if (Server.Get.GetPlayers(x => x.RoleType == RoleType.ClassD).Count() == 1)
                {
                    if (RoundStart.RoundLength.Seconds > 1)
                    {
                        foreach (Player ply in Server.Get.GetPlayers(x => x.RoleType == RoleType.ClassD))
                        {
                            Map.Get.SendBroadcast(5, $"<b><color=green>{ply.NickName}</color> wygrał!</b>");
                            Round.Get.RoundLock = false;
                            Timing.KillCoroutines("dmcheck");
                        }
                    }

                }
                yield return Timing.WaitForSeconds(5f);
            }
        }

        //Wojna Gangów
        public static void WojnaGangow()
        {
            foreach (Synapse.Api.Items.SynapseItem item in Map.Get.Items)
                item.Destroy();

            List<Player> players = Server.Get.Players.ToList();
            for (int i = 0; i < players.Count / 2; i++)
            {
                UnityEngine.Vector3 location3 = Map.Get.Rooms.First(x => x.RoomType == RoomName.EzCollapsedTunnel).Position;
                location3.y = location3.y + 1;

                team1.Add(players[i].UserId);
                players[i].RoleType = RoleType.Scientist;
                players[i].Inventory.Clear();
            }
            foreach (Door door in Map.Get.Doors)
            {
                if (GangWarDoors.Contains(door.DoorType))
                {
                    door.Locked = true;
                }
            }
            foreach (Player player in players)
            {
                UnityEngine.Vector3 location3 = Map.Get.Rooms.First(x => x.RoomType == RoomName.EzOfficeLarge).Position;
                location3.y = location3.y + 1;

                UnityEngine.Vector3 location2 = Map.Get.Rooms.First(x => x.RoomType == RoomName.EzOfficeStoried).Position;
                location2.y = location2.y + 1;

                Timing.CallDelayed(0.1f, () =>
                {
                    if (!team1.Contains(player.UserId))
                    {
                        player.RoleType = RoleType.ClassD;
                        player.Inventory.Clear();
                    }
                });
                Timing.CallDelayed(0.5f, () =>
                {
                    if (!team1.Contains(player.UserId))
                    {
                        player.AmmoBox[AmmoType.Ammo9x19] = 200;
                        player.AmmoBox[AmmoType.Ammo44cal] = 200;
                        player.AmmoBox[AmmoType.Ammo12gauge] = 200;
                        player.Inventory.AddItem(ItemType.GunCOM15);
                        player.Inventory.AddItem(ItemType.Medkit);
                        player.Inventory.AddItem(ItemType.SCP500);
                        player.Inventory.AddItem(ItemType.Adrenaline);
                    }
                    else
                    {
                        player.AmmoBox[AmmoType.Ammo9x19] = 200;
                        player.AmmoBox[AmmoType.Ammo44cal] = 200;
                        player.AmmoBox[AmmoType.Ammo12gauge] = 200;
                        player.Inventory.AddItem(ItemType.GunCOM15);
                        player.Inventory.AddItem(ItemType.Medkit);
                        player.Inventory.AddItem(ItemType.SCP500);
                        player.Inventory.AddItem(ItemType.Adrenaline);
                    }
                });
                Timing.CallDelayed(0.6f, () =>
                {
                    if (!team1.Contains(player.UserId))
                    {
                        player.Position = new Vector3(location2.x, location2.y, location2.z);
                    }
                    else
                    {
                        player.Position = new Vector3(location3.x, location3.y, location3.z);
                    }
                });
            }
        }

        //Hide And Seek
        public static void HideAndSeek()
        {
            Timing.RunCoroutine(HideAndSeekCheck(), "hascheck");
            int counter = 0;
            foreach (Player player in Server.Get.Players)
            {
                if (Server.Get.PlayersAmount < 5)
                {
                    if (counter > 0)
                    {
                        player.RoleType = RoleType.ClassD;
                        player.SendBroadcast(5, "<b>Jesteś <color=#32CD32>chowającym</color>, masz 30 sekund na schowanie się!</b>");
                    }
                    else
                    {
                        player.RoleType = RoleType.Scp93953;
                        Timing.CallDelayed(0.5f, () =>
                        {
                            player.Position = Extensions.GetRandomSpawnPoint(RoleType.Scp106);
                        });

                        player.SendBroadcast(5, "<b>Jesteś <color=red>szukającym</color>, za 30 sekund będziesz mógł szukać!</b>");
                        counter++;
                        Timing.CallDelayed(30, () =>
                        {
                            player.Position = Extensions.GetRandomSpawnPoint(RoleType.Scp173);
                            Map.Get.SendBroadcast(5, "<b><color=red>Szukający</color> zostali wypuszczeni!</b>");
                        });
                    }
                }
                else
                {
                    if (counter > 2)
                    {
                        player.RoleType = RoleType.ClassD;
                        player.SendBroadcast(5, "<b>Jesteś <color=#32CD32>chowającym</color>, masz 30 sekund na schowanie się!</b>");
                    }
                    else
                    {
                        player.RoleType = RoleType.Scp93953;
                        Timing.CallDelayed(0.5f, () =>
                        {
                            player.Position = Extensions.GetRandomSpawnPoint(RoleType.Scp106);
                        });
                        player.SendBroadcast(5, "<b>Jesteś <color=red>szukającym</color>, za 30 sekund będziesz mógł szukać!</b>");
                        counter++;
                        Timing.CallDelayed(30, () =>
                        {
                            player.Position = Extensions.GetRandomSpawnPoint(RoleType.Scp173);
                        });
                    }
                }

                Timing.CallDelayed(900, () =>
            {
                    if (Server.Get.GetPlayers(x => x.RoleType == RoleType.ClassD).Count() > 0)
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
                        foreach (Player scps in Server.Get.GetPlayers(x => x.RoleType == RoleType.Scp93953))
                        {
                            scps.Kill();
                        }
                        Map.Get.SendBroadcast(5, "<b><color=orange>Klasa-D</color> wygrała!</b>");
                    }
                    else if (Server.Get.GetPlayers(x => x.RoleType == RoleType.Scp93953).Count() > 0)
                    {
                        foreach (Player ply in Server.Get.GetPlayers(x => x.RoleType == RoleType.Scp93953))
                        {
                            if (pInfoDict.ContainsKey(ply.UserId))
                            {
                                PlayerInfo info = pInfoDict[ply.UserId];
                                info.Coins = info.Coins + 1;

                                pInfoDict[ply.UserId] = info;
                            }
                        }
                        Map.Get.SendBroadcast(5, "<b><color=red>SCP</color> wygrały!</b>");
                    }
            });
            }
        }

        private static IEnumerator<float> HideAndSeekCheck()
        {
            while (true)
            {
                if(RoundStart.RoundLength.Seconds > 5)
                {
                    if (Server.Get.GetPlayers(x => x.RoleType == RoleType.ClassD).Count() > 0 && Server.Get.GetPlayers(x => x.RoleType == RoleType.Scp93953).Count() == 0)
                    {
                        foreach (Player ply in Server.Get.Players)
                        {
                            if (ply.RoleType == RoleType.ClassD)
                            {
                                if (pInfoDict.ContainsKey(ply.UserId))
                                {
                                    PlayerInfo info = pInfoDict[ply.UserId];
                                    info.Coins = info.Coins + 1;

                                    pInfoDict[ply.UserId] = info;
                                }
                            }
                            else if (ply.RoleType == RoleType.Scp93953)
                            {
                                ply.Kill();
                            }
                        }
                        Map.Get.SendBroadcast(5, "<b><color=orange>Klasa-D</color> wygrała!</b>");
                        Timing.KillCoroutines("hascheck");
                    }
                    else if (Server.Get.GetPlayers(x => x.RoleType == RoleType.Scp93953).Count() > 0 && Server.Get.GetPlayers(x => x.RoleType == RoleType.ClassD).Count() == 0)
                    {
                        foreach (Player ply in Server.Get.Players)
                        {
                            if (ply.RoleType == RoleType.Scp93953)
                            {
                                if (pInfoDict.ContainsKey(ply.UserId))
                                {
                                    PlayerInfo info = pInfoDict[ply.UserId];
                                    info.Coins = info.Coins + 1;

                                    pInfoDict[ply.UserId] = info;
                                }
                            }
                        }
                        Map.Get.SendBroadcast(5, "<b><color=red>SCP</color> wygrały!</b>");
                        Timing.KillCoroutines("hascheck");
                    }
                }
                yield return Timing.WaitForSeconds(5f);
            }
        }

        //PropHunt

        public static void PropHunt()
        {
            List<Player> players = Server.Get.Players.ToList();
            for (int i = 0; i < players.Count / 2; i++)
            {
                players[i].RoleType = RoleType.ClassD;
                Handler.props.Add(players[i].UserId);
                players.Remove(players[i]);
            }

            foreach (Player player in players)
            {
                player.RoleType = RoleType.NtfCaptain;
                player.Inventory.Clear();

                Timing.CallDelayed(0.1f, () =>
                {
                    player.Inventory.AddItem(ItemType.GunCOM18);
                    player.AmmoBox[AmmoType.Ammo9x19] = 200;
                    player.AmmoBox[AmmoType.Ammo44cal] = 200;
                    player.AmmoBox[AmmoType.Ammo12gauge] = 200;
                    player.Position = Map.Get.Rooms.First(x => x.RoomType == RoomName.LczComputerRoom).Position;
                });
            }
        }
        //PeanutRun
        public static void WarheadRunn()
        {
            AlphaWarheadController.Host.StartDetonation();
            foreach (Player player in Server.Get.Players)
            {
                player.RoleType = RoleType.ClassD;
                player.GiveEffect(Effect.Scp207, 3);
            }

            Round.Get.RoundLock = true;
        }

        public static void DgBall()
        {
            Round.Get.RoundLock = true;
            Vector3 spawnPos = Extensions.GetRandomSpawnPoint(RoleType.Scp173);
            foreach (Player player in Server.Get.Players)
            {
                player.RoleType = RoleType.ClassD;
                Timing.CallDelayed(0.5f, () => player.Position = spawnPos);
            }

            foreach (Door door in Map.Get.Doors)
            {
                if (door.DoorType == DoorType.LCZ_173_Gate)
                {

                    door.Locked = true;
                }
            }
            Timing.RunCoroutine(DodgeballLoop(), "dgballLoop");
        }

        static IEnumerator<float> DodgeballLoop()
        {
            yield return Timing.WaitForSeconds(10f);

            for (; ; )
            {
                int count = 0;
                Player winner = null;
                foreach (Player player in Server.Get.Players)
                    if (!player.IsDead)
                    {
                        count++;
                        winner = player;
                    }

                if (count <= 1)
                {
                    Round.Get.RoundLock = false;
                    Map.Get.SendBroadcast(10, $"<B><color=green>{winner.NickName}</color> wygrał!</B>");
                    yield break;
                }

                foreach (Player player in Server.Get.Players)
                    if (rnd.Next(100) >= 50)
                        Map.Get.SpawnGrenade(player.Position, Vector3.zero, 3, GrenadeType.Scp018, player);

                yield return Timing.WaitForSeconds(5.5f);
            }
        }
    }
}
