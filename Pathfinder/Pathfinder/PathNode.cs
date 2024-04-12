using System.Collections.Generic;

namespace Pathfinder.Logic
{
    internal record PathNode
    {
        private decimal? length;

        public PathNode(Coordinate Coordinate, PathNode? Predecessor, NodeState NodeState)
        {
            this.Coordinate = Coordinate;
            this.Predecessor = Predecessor;
            this.NodeState = NodeState;
        }

        public Coordinate Coordinate { get; }
        public PathNode Predecessor { get; }
        public NodeState NodeState { get; }

        public decimal GetLength(IDistanceCalculator distanceCalculator, Board board)
        {
            if (length.HasValue)
                return length.Value;
            if (Predecessor?.length is not null)
            {
                length = distanceCalculator.GetDistanceToNeighbor(Predecessor, this, board) + Predecessor.length;
                return length.Value;
            }
            length = 0;
            var node = this;
            while (node.Predecessor is not null)
            {
                length += distanceCalculator.GetDistanceToNeighbor(node.Predecessor, node, board);
                node = node.Predecessor;
            }
            return length.Value;
        }

        public Path ToPath(IDistanceCalculator distanceCalculator, Board board)
        {
            var coordinates = new Stack<Coordinate>();
            var pathNode = this;
            do
            {
                coordinates.Push(pathNode.Coordinate);
                pathNode = pathNode.Predecessor;
            } while (pathNode != null);
            return new()
            {
                PathCoordinates = [.. coordinates],
                Length = GetLength(distanceCalculator, board),
            };
        }
    }
}