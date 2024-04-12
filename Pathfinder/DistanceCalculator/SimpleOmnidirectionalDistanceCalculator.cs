using System;

namespace Pathfinder.Logic
{
    internal class SimpleOmnidirectionalDistanceCalculator : AirDistanceBasedCalculator
    {
        protected override decimal GetAirDistance(Coordinate a, Coordinate b)
        {
            var xDifference = Math.Abs(a.X - b.X);
            var yDifference = Math.Abs(a.Y - b.Y);
            return Math.Max(xDifference, yDifference);
        }
    }
}