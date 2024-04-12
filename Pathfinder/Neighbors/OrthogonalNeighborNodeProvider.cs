using System.Collections.Generic;
using System.Linq;

namespace Pathfinder.Logic
{
    internal class OrthogonalNeighborNodeProvider : INeighborNodeProvider
    {
        public IEnumerable<PathNode> GetAccessibleNeighbors(Board board, PathNode node)
        {
            var orthogonalNeighbors = node.Coordinate.GetOrthogonalNeighborCoordinates();
            return orthogonalNeighbors.Where(board.CoordinateIsAccessible).Select(c => new PathNode(c, node, node.NodeState));
        }
    }
}