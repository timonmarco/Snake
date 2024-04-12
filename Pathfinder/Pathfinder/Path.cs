using System;

namespace Pathfinder.Logic
{
    public class Path
    {
        public Coordinate[] PathCoordinates { get; set; } = Array.Empty<Coordinate>();
        public decimal Length { get; set; }
    }
}