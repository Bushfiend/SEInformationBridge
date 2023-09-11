using Sandbox.Engine.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VRage.Game;

namespace SEInformationBridge
{
    public static class ServerInfo
    {


        public static List<ServerSettings> GetServerSettings()
        {
            if (Plugin.TorchInstance.CurrentSession == null)
                return null;
            var list = new List<ServerSettings>();

            list.Add(new ServerSettings(MyMultiplayer.Static));

            return list;

        }


        public class ServerSettings
        {

            public string WorldName { get; set; }
            public string GameMode { get; set; }
            public int MaxPlayers { get; set; }
            public string HostName { get; set; }
            public ulong WorldSize { get; set; }
            public int SyncDistance { get; set; }
            public bool IsServerExperimental { get; set; }
            public float InventoryMultiplier { get; set; }
            public float BlocksInventoryMultiplier { get; set; }
            public float AssemblerMultiplier { get; set; }
            public float RefineryMultiplier { get; set; }
            public float WelderMultiplier { get; set; }
            public float GrinderMultiplier { get; set; }
            public List<ModItem> Mods { get; set; }
            public Dictionary<ulong, string> BannedClients { get; set; }

            public ServerSettings(MyMultiplayerBase server)
            {
                WorldName = server.WorldName;
                GameMode = server.GameMode.ToString();
                MaxPlayers = server.MaxPlayers;
                IsServerExperimental = server.IsServerExperimental;
                InventoryMultiplier = server.InventoryMultiplier;
                BlocksInventoryMultiplier = server.BlocksInventoryMultiplier;
                AssemblerMultiplier = server.AssemblerMultiplier;
                RefineryMultiplier = server.RefineryMultiplier;
                WelderMultiplier = server.WelderMultiplier;
                GrinderMultiplier = server.GrinderMultiplier;
                HostName = server.HostName;
                WorldSize = server.WorldSize;
                SyncDistance = server.SyncDistance;
                Mods = GenerateModList(server);
                BannedClients = GetBannedPlayers(server);
            }

            internal Dictionary<ulong, string> GetBannedPlayers(MyMultiplayerBase server)
            {
                var bannedPlayers = server.BannedClients;
                if (bannedPlayers.Count == 0)
                    return null;

                Dictionary<ulong, string> players = new Dictionary<ulong, string>();
                foreach (var player in bannedPlayers)
                {
                    var name = string.Empty;
                    name = MySession.Static.Players.TryGetIdentityNameFromSteamId(player);

                    players.Add(player, name);
                }
                return players;
            }

            internal List<ModItem> GenerateModList(MyMultiplayerBase server)
            {
                var mods = server.Mods;
                if (mods.Count == 0)
                    return null;

                var modItemList = new List<ModItem>();
                foreach(var mod in mods)
                    modItemList.Add(new ModItem(mod));

                return modItemList;
            }

            public class ModItem
            {
                public string Name { get; set; }
                public ulong Id { get; set; }
                public string Link { get; set; }

                public ModItem(MyObjectBuilder_Checkpoint.ModItem mod)
                {
                    Name = mod.FriendlyName;
                    Id = mod.PublishedFileId;
                    Link = $"https://steamcommunity.com/sharedfiles/filedetails/?id={mod.PublishedFileId}";
                }

            }



        }






    }
}
