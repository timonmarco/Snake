using System.Collections.Generic;
using System.Linq;

namespace Pathfinder.Logic
{
    internal class TeleporterNeighborNodeProvider : INeighborNodeProvider
    {
        public IEnumerable<PathNode> GetAccessibleNeighbors(Board board, PathNode node)
        {
            var teleporters = board.GetTeleporters();
            if (!teleporters.Contains(node.Coordinate))
                return Enumerable.Empty<PathNode>();
            return teleporters.Where(c => c != node.Coordinate).Select(c => new PathNode(c, node, node.NodeState));
        }
    }
}