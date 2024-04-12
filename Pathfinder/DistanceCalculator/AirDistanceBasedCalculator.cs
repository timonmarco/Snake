using System;
using System.Linq;

namespace Pathfinder.Logic
{
    internal abstract class AirDistanceBasedCalculator : IDistanceCalculator
    {
        public decimal GetDistanceToNeighbor(PathNode node, PathNode neighbor, Board board)
        {
            var distanceMultiplier = GetAverageTerrainDistanceMultiplier(node, neighbor, board);
            return distanceMultiplier * GetAirDistance(node.Coordinate, neighbor.Coordinate) + GetPickupCost(node, neighbor);
        }

        private static decimal GetPickupCost(PathNode node, PathNode neighbor) => node.NodeState.GetItems().Except(neighbor.NodeState.GetItems()).Sum(i => i.PickupCost);

        public decimal GetMinimumDistance(Coordinate a, Coordinate b, Board board)
        {
            var teleporters = board.GetTeleporters();
            decimal minDistance = GetAirDistance(a, b);
            if (teleporters.Length != 0)
            {
                var distanceTeleportersToA = teleporters.Min(t => GetAirDistance(a, t));
                var distanceTeleportersToB = teleporters.Min(t => GetAirDistance(b, t));
                minDistance = Math.Min(minDistance, distanceTeleportersToA + distanceTeleportersToB);
            }
            return board.LowestTerrainMultiplier * minDistance;
        }

        private static decimal GetAverageTerrainDistanceMultiplier(PathNode node, PathNode neighbor, Board board)
        {
            var teleporters = board.GetTeleporters().ToHashSet();
            if (teleporters.Contains(node.Coordinate) && teleporters.Contains(neighbor.Coordinate))
                return 0;
            var items = neighbor.NodeState.GetItems();
            return (GetTerrainDistanceMultiplier(board.GetTerrain(node.Coordinate), items) + GetTerrainDistanceMultiplier(board.GetTerrain(neighbor.Coordinate), items)) / 2M;
        }

        private static decimal GetTerrainDistanceMultiplier(Terrain terrain, Item[] items) => items.Length == 0 ? terrain.GetDistanceMultiplier() :
            items.Select(item => item.GetDistanceFactor(terrain)).Aggregate((f1, f2) => f1 * f2) * terrain.GetDistanceMultiplier();

        protected abstract decimal GetAirDistance(Coordinate a, Coordinate b);
    }
}