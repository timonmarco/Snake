using System;

namespace Pathfinder.Logic
{
    internal class SimpleOrthogonalDistanceCalculator : AirDistanceBasedCalculator
    {
        protected override decimal GetAirDistance(Coordinate a, Coordinate b)
        {
            var xDifference = Math.Abs(a.X - b.X);
            var yDifference = Math.Abs(a.Y - b.Y);
            return xDifference + yDifference;
        }
    }
}