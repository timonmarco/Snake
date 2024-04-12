using System;
namespace Pathfinder.Logic
{
    internal class GeometricDistanceCalculator : AirDistanceBasedCalculator
    {
        private static readonly decimal diagonalFactor = (decimal)Math.Sqrt(2);

        protected override decimal GetAirDistance(Coordinate a, Coordinate b)
        {
            var xDifference = Math.Abs(a.X - b.X);
            var yDifference = Math.Abs(a.Y - b.Y);
            return diagonalFactor * Math.Min(xDifference, yDifference) + Math.Abs(xDifference - yDifference);
        }
    }
}