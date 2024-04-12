using System.Collections.Generic;
using System.Linq;

namespace Pathfinder.Logic
{
    internal class DiagonalNeighborNodeProvider : INeighborNodeProvider
    {
        public IEnumerable<PathNode> GetAccessibleNeighbors(Board board, PathNode node)
        {
            var accessibleOrthogonalNeighbors = node.Coordinate.GetOrthogonalNeighborCoordinates().Where(board.CoordinateIsAccessible).ToHashSet();
            var diagonalNeighbors = node.Coordinate.GetDiagonalNeighborCoordinatesWithSharedNeighbors();
            return diagonalNeighbors.Where(DiagonalIsAccessible).Select(pair => pair.neighbor).Select(c => new PathNode(c, node, node.NodeState));

            bool DiagonalIsAccessible((Coordinate neighbor, Coordinate[] sharedNeighbors) pair)
            {
                if (!board.CoordinateIsAccessible(pair.neighbor))
                    return false;
                return pair.sharedNeighbors.Any(accessibleOrthogonalNeighbors.Contains);
            }
        }
    }
}