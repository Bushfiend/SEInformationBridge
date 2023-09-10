using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Xml.Serialization;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRageMath;


namespace SEInformationBridge
{
    public static class GridInfo
    {
        public static Dictionary<long, GridEntity> Grids = new Dictionary<long, GridEntity>();

        public static void Setup()
        {
            MyAPIGateway.Entities.OnEntityAdd += OnEntityAdded;
            MyAPIGateway.Entities.OnEntityRemove += OnEntityRemoved;
            GetGrids();
        }

        public static void GetGrids()
        {
            var grids = MyEntities.GetEntities().OfType<MyCubeGrid>().ToList();
            foreach(var grid in grids)
            {
                if (Grids.ContainsKey(grid.EntityId))
                    continue;
                
                Grids.Add(grid.EntityId, new GridEntity(grid));
            }
            
        }

        public static string Serialize()
        {
            if(Grids.Count == 0)
                return "No Grids/Server Offline";

            UpdateInfo();

            List<GridEntity> values = new List<GridEntity>();
            values.AddRange(Grids.Values);


            return JsonSerializer.Serialize(values, new JsonSerializerOptions { WriteIndented = true });      
        }
        
        
        private static void OnEntityAdded(IMyEntity entity)
        {
            if ( !(entity is MyCubeGrid) )
                return;

            if (Grids.ContainsKey(entity.EntityId))
                return;

            Grids.Add(entity.EntityId, new GridEntity(entity as MyCubeGrid));
        }

        private static void OnEntityRemoved(IMyEntity entity)
        {
            if (!(entity is MyCubeGrid))
                return;

            if (Grids.ContainsKey(entity.EntityId))
            {
                Grids.Remove(entity.EntityId);
            }
        }

        private static void UpdateInfo()
        {
            foreach(var grid in Grids)
            {
                grid.Value.UpdateGridInfo();
            }
        }

       
        public class GridEntity
        {
            private MyCubeGrid Grid { get; set; }         
            public string Name { get; set; }
            public string BlockSize { get; set; }
            public string BigOwnerName { get; set; }
            public string FactionTag { get; set; }
            public long EntityId { get; set; }
            public int BlockCount { get; set; }
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }

            public GridEntity(MyCubeGrid grid) 
            {
                Grid = grid;
                UpdateGridInfo();
            }

            public void UpdateGridInfo()
            {
                Name = Grid.DisplayName;

                var bigOwner = Grid.BigOwners.FirstOrDefault();
                if (bigOwner == 0)
                {
                    BigOwnerName = "No Owner";                   
                }
                else
                {
                    var owner = MySession.Static.Players.TryGetIdentity(bigOwner);
                    BigOwnerName = owner.DisplayName;
                    var faction = MySession.Static.Factions.GetPlayerFaction(bigOwner);
                    if (faction != null)
                    {
                        FactionTag = faction.Tag;
                    }
                }

                EntityId = Grid.EntityId;
                BlockCount = Grid.BlocksCount;              
                BlockSize = Grid.GridSizeEnum.ToString();
                X = (float)Math.Round(Grid.PositionComp.GetPosition().X, 2);
                Y = (float)Math.Round(Grid.PositionComp.GetPosition().Y, 2);
                Z = (float)Math.Round(Grid.PositionComp.GetPosition().Z, 2);

            }
       

        }





    }

}
