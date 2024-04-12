using System;

namespace Pathfinder.Logic
{
    internal class AStarMetric(IDistanceCalculator distanceCalculator) : INodeMetric
    {
        private readonly IDistanceCalculator distanceCalculator = distanceCalculator ?? throw new ArgumentNullException(nameof(distanceCalculator));

        //The node with the shorter expected distance is better
        public decimal GetValue(PathNode node, Board board) => node.GetLength(distanceCalculator, board) + distanceCalculator.GetMinimumDistance(node.Coordinate,
            board.End ?? throw new ArgumentException($"The {nameof(Board)} must have an {nameof(Board.End)}", nameof(board)),
            board);
    }
}