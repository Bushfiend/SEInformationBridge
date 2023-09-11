using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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



            }


        }







    }
}
