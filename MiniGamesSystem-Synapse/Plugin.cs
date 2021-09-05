using System;
using Exiled.API.Features;
using sc = SynapseController;
using PlayerEv = Exiled.Events.Handlers.Player;
using MapEv = Exiled.Events.Handlers.Map;
using WarheadEv = Exiled.Events.Handlers.Warhead;
using UnityEngine;
using System.IO;
using Synapse.Api.Plugin;
using Synapse.Api;

namespace MiniGamesSystem
{
    [PluginInformation(
       Name = "MiniGamesSystem",
       Author = "Naku (Cwaniaak.)",
       Description = "MiniGamesSystem for synapse",
       LoadPriority = 0,
       SynapseMajor = 2,
       SynapseMinor = 6,
       SynapsePatch = 0,
       Version = "v.1.0.0"
       )]
    public class MiniGamesSystem : AbstractPlugin
    {
        public static GameObject workstationObj;
        public static string DataPath = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), "Plugins"), "MiniGamesSystemData");
        private static readonly Lazy<MiniGamesSystem> LazyInstance = new Lazy<MiniGamesSystem>(() => new MiniGamesSystem());
        public static MiniGamesSystem Instance => LazyInstance.Value;

        [Config(section = "MiniGamesSystem")]
        public static PluginConfig Config;
        private MiniGamesSystem() { }
        private Handler handler;

        public override void Load()
        {
            base.Load();
            handler = new Handler();

            if (!Directory.Exists(DataPath)) Directory.CreateDirectory(DataPath);

            sc.Server.Events.Map.WarheadDetonationEvent += handler.OnWarheadDetonated;
            WarheadEv.Detonated += handler.OnWarheadDetonated;
            MapEv.SpawningItem += handler.OnSpawningItems;
            ServerEv.WaitingForPlayers += handler.OnWTP;
            PlayerEv.Verified += handler.OnJoin;
            ServerEv.RoundStarted += handler.OnRS;
            PlayerEv.PickingUpItem += handler.OnPickingUp;
            ServerEv.RestartingRound += handler.OnRoundRestart;
            PlayerEv.Shooting += handler.OnShooting;
            ServerEv.EndingRound += handler.OnRndEnd;
            ServerEv.RespawningTeam += handler.OnRespawning;
            WarheadEv.Stopping += handler.OnWarheadCancel;
            PlayerEv.Died += handler.OnPlyDied;
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            WarheadEv.Detonated -= handler.OnWarheadDetonated;
            MapEv.SpawningItem -= handler.OnSpawningItems;
            ServerEv.WaitingForPlayers -= handler.OnWTP;
            PlayerEv.Verified -= handler.OnJoin;
            ServerEv.RoundStarted -= handler.OnRS;
            PlayerEv.PickingUpItem -= handler.OnPickingUp;
            ServerEv.RestartingRound -= handler.OnRoundRestart;
            PlayerEv.Shooting -= handler.OnShooting;
            ServerEv.EndingRound -= handler.OnRndEnd;
            ServerEv.RespawningTeam -= handler.OnRespawning;
            WarheadEv.Stopping -= handler.OnWarheadCancel;
            PlayerEv.Died -= handler.OnPlyDied;
            handler = null;
        }

        public enum ObjectType
        {
            WorkStation,
            DoorLCZ,
            DoorHCZ,
            DoorEZ
        }

        public static GameObject GetWorkStationObject(ObjectType type)
        {
            GameObject bench = null;
            bench = UnityEngine.Object.Instantiate(workstationObj);
            return bench;
        }
    }
}
