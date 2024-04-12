using System;
using System.Collections.Generic;

namespace Pathfinder.Logic
{
    public partial class PathStepTakenEventArgs : EventArgs
    {
        public HashSet<Coordinate> NewlyVisitedCoordinate { get; set; } = [];
        public HashSet<Coordinate> NewlyLivingCoordinates { get; set; } = [];
    }
}