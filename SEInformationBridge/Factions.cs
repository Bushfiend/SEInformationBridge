using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using static SEInformationBridge.PlayerGrids;

namespace SEInformationBridge
{
    public static class Factions
    {

        public static List<FactionInfo> GetFactions()
        {
            if (Plugin.TorchInstance.CurrentSession == null)
                return null;


            var factionList = MySession.Static.Factions.GetAllFactions().ToList();

            if (factionList.Count == 0)
                return null;

            var factionInfo = new List<FactionInfo>();
            foreach (var faction in factionList)
            {
                factionInfo.Add(new FactionInfo(faction));
            }

            return factionInfo;
        }

        public class FactionInfo
        {
            public string Name { get; set; }
            public string Tag { get; set; }
            public int MemberCount { get; set; }
            public string LeaderName { get; set; }

            public FactionInfo(MyFaction faction)
            {
                Name = faction.Name;
                Tag = faction.Tag;
                MemberCount = faction.Members.Count;
                LeaderName = MySession.Static.Players.TryGetIdentity(faction.FounderId).DisplayName;
            }
        }




    }
}
