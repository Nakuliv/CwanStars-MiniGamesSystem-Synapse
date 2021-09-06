using System;
using sc = SynapseController;
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
            sc.Server.Events.Map.WarheadDetonationEvent += handler.OnWarheadDetonated;
            sc.Server.Events.Round.WaitingForPlayersEvent += handler.OnWTP;
            sc.Server.Events.Player.PlayerJoinEvent += handler.OnJoin;
            sc.Server.Events.Round.RoundStartEvent += handler.OnRS;
            sc.Server.Events.Player.PlayerPickUpItemEvent += handler.OnPickingUp;
            sc.Server.Events.Round.RoundRestartEvent += handler.OnRoundRestart;
            sc.Server.Events.Player.PlayerShootEvent += handler.OnShooting;
            sc.Server.Events.Round.RoundEndEvent += handler.OnRndEnd;
            sc.Server.Events.Round.TeamRespawnEvent += handler.OnRespawning;
            sc.Server.Events.Player.PlayerDeathEvent += handler.OnPlyDied;
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
