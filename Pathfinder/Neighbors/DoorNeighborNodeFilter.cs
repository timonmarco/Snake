using System;
using System.Collections.Generic;

namespace Pathfinder.Logic
{
    internal class DoorNeighborNodeFilter(INeighborNodeProvider innerProvider) : INeighborNodeProvider
    {
        private readonly INeighborNodeProvider innerProvider = innerProvider ?? throw new ArgumentNullException(nameof(innerProvider));

        public IEnumerable<PathNode> GetAccessibleNeighbors(Board board, PathNode node)
        {
            foreach (var neighbor in innerProvider.GetAccessibleNeighbors(board, node))
            {
                var door = board.GetDoor(neighbor.Coordinate);
                if (door < 0)
                {
                    yield return neighbor;
                    continue;
                }
                if (neighbor.NodeState.HasItem(Item.GetKey((ushort)door)))
                    yield return neighbor;
            }
        }
    }
}