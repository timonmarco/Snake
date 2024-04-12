using System.Collections.Generic;
using System.Linq;

namespace Pathfinder.Logic
{
    internal class DiagonalGhostNeighborNodeProvider : INeighborNodeProvider
    {
        public IEnumerable<PathNode> GetAccessibleNeighbors(Board board, PathNode node)
        {
            var diagonalNeighbors = node.Coordinate.GetDiagonalNeighborCoordinates();
            return diagonalNeighbors.Where(c => board.GetTerrain(c) != Terrain.Obstacle).Select(c => new PathNode(c, node, node.NodeState));
        }
    }
}