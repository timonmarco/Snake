using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinder.Logic
{
    /// <summary>
    /// Adds nodes to a sorted dictionary using a metric defined by <see cref="INodeMetric"/> to determine the order of nodes.
    /// </summary>
    internal class BestFirstNodeStorage(INodeMetric nodeMetric) : INodeStorage
    {
        private readonly INodeMetric nodeMetric = nodeMetric ?? throw new ArgumentNullException(nameof(nodeMetric));
        private readonly SortedDictionary<decimal, Stack<PathNode>> nodes = [];

        public void AddNode(PathNode node, Board board)
        {
            var nodeValue = nodeMetric.GetValue(node, board);
            var equalValueNodes = GetEqualValueNodes(nodeValue);
            equalValueNodes.Push(node);
        }

        private Stack<PathNode> GetEqualValueNodes(decimal nodeValue)
        {
            if (nodes.TryGetValue(nodeValue, out var equalValueNodes))
                return equalValueNodes;
            equalValueNodes = new();
            nodes.Add(nodeValue, equalValueNodes);
            return equalValueNodes;
        }

        public PathNode? PopNextNode()
        {
            if (nodes.Count == 0)
                return null;
            var node = nodes.First();
            var nextNode = node.Value.Pop();
            if (node.Value.Count == 0)
                nodes.Remove(node.Key);
            return nextNode;
        }
    }
}