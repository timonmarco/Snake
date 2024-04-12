using System;

namespace Pathfinder.Logic
{
    internal class BreadthFirstMetric(IDistanceCalculator distanceCalculator) : INodeMetric
    {
        private readonly IDistanceCalculator distanceCalculator = distanceCalculator ?? throw new ArgumentNullException(nameof(distanceCalculator));

        //Newer nodes are always worse than old nodes
        public decimal GetValue(PathNode node, Board board) => node.GetLength(distanceCalculator, board);
    }
}