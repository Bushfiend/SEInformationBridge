using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VRageMath;
using static SEInformationBridge.Factions;

namespace SEInformationBridge
{
    public static class Players
    {
      
        public static List<PlayerInfo> GetPlayers()
        {
            if (Plugin.TorchInstance.CurrentSession == null)
                return null;

            var players = MySession.Static.Players.GetAllIdentities();

            var playerInfo = new List<PlayerInfo>();

            foreach (var player in players)
            {
                playerInfo.Add(new PlayerInfo(player));
            }
            return playerInfo;
        }

        public class PlayerInfo
        {
            public string Name { get; set; }
            public long IdentityId { get; set; }
            public ulong SteamId { get; set; }
            public string FactionName { get; set; }
            public long FactionId { get; set; }
            public Vector Location { get; set; }
            public bool IsOnline { get; set; }
            public float TimeSpentDrilling { get; set; }
            public long TotalDamageTaken { get; set; }
            

            public PlayerInfo(MyIdentity player)
            {
                Name = player.DisplayName;
                IdentityId = player.IdentityId;
                SteamId = MySession.Static.Players.TryGetSteamId(player.IdentityId);

                var faction = MySession.Static.Factions.GetPlayerFaction(player.IdentityId);
                if (faction == null)
                {
                    FactionName = "None";
                    FactionId = 0;
                    return;
                }

                FactionName = faction.Name;
                FactionId = faction.FactionId;
                MyPlayer myPlayer;
                if (MySession.Static.Players.TryGetPlayerBySteamId(SteamId, out myPlayer))
                {
                    IsOnline = MySession.Static.Players.GetOnlinePlayers().Contains(myPlayer);
                    TimeSpentDrilling = myPlayer.TimeSpendMining;
                    Location = new Vector(myPlayer.GetPosition());
                    TotalDamageTaken = myPlayer.TotalDamageTaken;
                }
                else
                {
                    IsOnline = false;
                    TimeSpentDrilling = 0f;
                    Location = new Vector(Vector3D.Zero);
                    TotalDamageTaken = 0L;
                }
                
                
                  
            }
            public class Vector
            {
                public float X { get; set; }
                public float Y { get; set; }
                public float Z { get; set; }
                public Vector(Vector3D vec)
                {
                    X = (float)Math.Round(vec.X);
                    Y = (float)Math.Round(vec.Y);
                    Z = (float)Math.Round(vec.Z);
                }
            }

        }


    }
}
