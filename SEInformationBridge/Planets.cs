using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using static SEInformationBridge.Factions;
using VRageMath;

namespace SEInformationBridge
{
    public static class Planets
    {
        public static List<PlanetInfo> GetPlanets()
        {
            if (Plugin.TorchInstance.CurrentSession == null)
                return null;

            var planets = MyEntities.GetEntities().OfType<MyPlanet>().ToList();
            if (planets.Count == 0)
                return null;

            var planetInfo = new List<PlanetInfo>();

            foreach (var planet in planets)
            {
                planetInfo.Add(new PlanetInfo(planet));
            }

            return planetInfo;
        }

        public class PlanetInfo
        {
            public string Name { get; set; }
            public float Radius { get; set; }
            public long EntityId { get; set; }
            public bool HasAtmosphere { get; set; }
            public bool HasOxygen { get; set; }
            public float Gravity { get; set; }
            public Vector Location { get; set; }

            public PlanetInfo(MyPlanet planet)
            {
                Name = planet.Generator.Id.SubtypeId.ToString();
                Radius = planet.AverageRadius;
                EntityId = planet.EntityId;
                HasAtmosphere = planet.HasAtmosphere;
                HasOxygen = planet.Generator.Atmosphere.Breathable;
                Gravity = planet.Generator.SurfaceGravity;
                Location = new Vector(planet.PositionComp.GetPosition());
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
