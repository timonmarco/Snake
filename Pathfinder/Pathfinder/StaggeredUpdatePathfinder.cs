using System;
using System.Collections.Generic;

namespace Pathfinder.Logic
{
    /// <summary>
    /// Provides staggered instead of live updates with the <see cref="PathStepTaken"/> event. The number of steps between each event increases exponentially
    /// </summary>
    public class StaggeredUpdatePathfinder : IPathfinder
    {
        private static readonly double stepBase = 1.02;
        private readonly IPathfinder innerPathfinder;
        private double pathStepCount;
        private double pathStepCountAtLastUpdate;
        private readonly HashSet<Coordinate> visitedCoordinatesSinceLastUpdate = [];
        private readonly HashSet<Coordinate> livingCoordinatesSinceLastUpdate = [];

        public event EventHandler<PathStepTakenEventArgs>? PathStepTaken;

        public PathStepTakenEventArgs CurrentEventArgs => new() { NewlyLivingCoordinates = [.. livingCoordinatesSinceLastUpdate], NewlyVisitedCoordinate = [.. visitedCoordinatesSinceLastUpdate] };

        public StaggeredUpdatePathfinder(IPathfinder innerPathfinder)
        {
            this.innerPathfinder = innerPathfinder ?? throw new ArgumentNullException(nameof(innerPathfinder));
            innerPathfinder.PathStepTaken += OnInnerPathStepTaken;
        }

        public Path? FindPath(Board board)
        {
            pathStepCount = 0;
            pathStepCountAtLastUpdate = 0;
            return innerPathfinder.FindPath(board);
        }

        private void OnInnerPathStepTaken(object? sender, PathStepTakenEventArgs e)
        {
            livingCoordinatesSinceLastUpdate.UnionWith(e.NewlyLivingCoordinates);
            livingCoordinatesSinceLastUpdate.ExceptWith(e.NewlyVisitedCoordinate);
            visitedCoordinatesSinceLastUpdate.UnionWith(e.NewlyVisitedCoordinate);
            pathStepCount++;
            if (pathStepCount < stepBase * pathStepCountAtLastUpdate)
                return;
            pathStepCountAtLastUpdate = pathStepCount;
            PathStepTaken?.Invoke(this, CurrentEventArgs);
            visitedCoordinatesSinceLastUpdate.Clear();
            livingCoordinatesSinceLastUpdate.Clear();
        }
    }
}
