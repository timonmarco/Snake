using System;

namespace Pathfinder.Logic
{
    public static class TerrainMethods
    {
        public static decimal GetDistanceMultiplier(this Terrain terrain) => terrain switch
        {
            Terrain.Empty => 1,
            Terrain.Water => 2,
            Terrain.Street => 0.5M,
            Terrain.Obstacle => throw new InvalidOperationException("Obstacles don't have a terrain value."),
            _ => throw new NotImplementedException(),
        };
    }
}