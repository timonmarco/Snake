using System.Collections.Generic;
namespace Pathfinder.Logic
{
    internal class BreadthFirstNodeStorage : INodeStorage
    {
        private readonly Queue<PathNode> nodes = new();

        public void AddNode(PathNode node, Board board) => nodes.Enqueue(node);

        public PathNode? PopNextNode() => nodes.Count != 0 ? nodes.Dequeue() : null;
    }
}