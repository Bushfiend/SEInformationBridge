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
    public static class PlayerGrids
    {
        public static Dictionary<long, GridInfo> Grids = new Dictionary<long, GridInfo>();

        public static void Setup()
        {
            MyAPIGateway.Entities.OnEntityAdd += OnEntityAdded;
            MyAPIGateway.Entities.OnEntityRemove += OnEntityRemoved;
            UpdateGrids();
        }

        public static List<GridInfo> GetGridList()
        {
            if (Plugin.TorchInstance.CurrentSession == null)
                return null;

            UpdateGrids();

            var gridList = new List<GridInfo>();
            gridList.AddRange(Grids.Values);
            return gridList;
        }

        public static void UpdateGrids()
        {
            var grids = MyEntities.GetEntities().OfType<MyCubeGrid>().ToList();
            foreach(var grid in grids)
            {
                if (Grids.ContainsKey(grid.EntityId))
                    continue;
                
                Grids.Add(grid.EntityId, new GridInfo(grid));
            }
            UpdateInfo();
        }
        
        private static void OnEntityAdded(IMyEntity entity)
        {
            if ( !(entity is MyCubeGrid) )
                return;

            if (Grids.ContainsKey(entity.EntityId))
                return;

            Grids.Add(entity.EntityId, new GridInfo(entity as MyCubeGrid));
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

       
        public class GridInfo
        {
            private MyCubeGrid Grid { get; set; }         
            public string Name { get; set; }
            public string BlockSize { get; set; }
            public string BigOwnerName { get; set; }
            public string FactionTag { get; set; }
            public long EntityId { get; set; }
            public int BlockCount { get; set; }
            public Vector Location { get; set; }

            public GridInfo(MyCubeGrid grid) 
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
                    if (faction == null)
                    {
                        FactionTag = "No Faction";
                    }
                    else
                    {
                        FactionTag = faction.Tag;
                    }
                }

                EntityId = Grid.EntityId;
                BlockCount = Grid.BlocksCount;              
                BlockSize = Grid.GridSizeEnum.ToString();
                Location = new Vector(Grid.PositionComp.GetPosition());

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
