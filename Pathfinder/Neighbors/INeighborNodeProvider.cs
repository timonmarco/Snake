using System.Collections.Generic;

namespace Pathfinder.Logic
{
    internal interface INeighborNodeProvider
    {
        IEnumerable<PathNode> GetAccessibleNeighbors(Board board, PathNode node);
    }
}