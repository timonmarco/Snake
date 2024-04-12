using System.Collections.Generic;
using System.Linq;

namespace Pathfinder.Logic
{
    internal class ItemPickupNeighborNodeProvider : INeighborNodeProvider
    {
        public IEnumerable<PathNode> GetAccessibleNeighbors(Board board, PathNode node)
        {
            foreach (var item in board.GetItems(node.Coordinate).Where(i => !node.NodeState.HasItem(i)))
                yield return new PathNode(node.Coordinate, node, new NodeState(node.NodeState.GetItems().Append(item)));
        }
    }
}