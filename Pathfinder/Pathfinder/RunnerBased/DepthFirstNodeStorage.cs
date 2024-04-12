using System.Collections.Generic;

namespace Pathfinder.Logic
{
    internal class DepthFirstNodeStorage : INodeStorage
    {
        private readonly Stack<PathNode> nodes = new();

        public void AddNode(PathNode node, Board board) => nodes.Push(node);

        public PathNode? PopNextNode() => nodes.Count != 0 ? nodes.Pop() : null;
    }
}