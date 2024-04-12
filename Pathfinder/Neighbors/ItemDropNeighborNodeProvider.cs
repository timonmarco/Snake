using System.Collections.Generic;
using System.Linq;

namespace Pathfinder.Logic
{
    internal class ItemDropNeighborNodeProvider : INeighborNodeProvider
    {
        public IEnumerable<PathNode> GetAccessibleNeighbors(Board board, PathNode node)
        {
            foreach (var item in node.NodeState.GetItems())
                yield return new PathNode(node.Coordinate, node, new NodeState(node.NodeState.GetItems().Where(i => i != item)));
        }
    }
}