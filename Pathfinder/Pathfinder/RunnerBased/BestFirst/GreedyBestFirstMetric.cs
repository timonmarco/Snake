using System;

namespace Pathfinder.Logic
{
    internal class GreedyBestFirstMetric(IDistanceCalculator distanceCalculator) : INodeMetric
    {
        private readonly IDistanceCalculator distanceCalculator = distanceCalculator ?? throw new ArgumentNullException(nameof(distanceCalculator));

        //The node closer to the end is better
        public decimal GetValue(PathNode node, Board board) => distanceCalculator.GetMinimumDistance(node.Coordinate,
            board.End ?? throw new ArgumentException($"The {nameof(Board)} must have an {nameof(Board.End)}", nameof(board)),
            board);
    }
}