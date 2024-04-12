using System;

namespace Pathfinder.Logic
{
    internal class AntiAStarMetric(IDistanceCalculator distanceCalculator) : INodeMetric
    {
        private readonly IDistanceCalculator distanceCalculator = distanceCalculator ?? throw new ArgumentNullException(nameof(distanceCalculator));

        public decimal GetValue(PathNode node, Board board) => -(node.GetLength(distanceCalculator, board) + distanceCalculator.GetMinimumDistance(node.Coordinate,
            board.End ?? throw new ArgumentException($"The {nameof(Board)} must have an {nameof(Board.End)}", nameof(board)),
            board));
    }
}