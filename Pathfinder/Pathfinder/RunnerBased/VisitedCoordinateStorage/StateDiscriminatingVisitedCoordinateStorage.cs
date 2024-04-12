using System.Collections.Generic;

namespace Pathfinder.Logic
{
    internal class StateDiscriminatingVisitedCoordinateStorage : IVisitedCoordinateStorage
    {
        private readonly Dictionary<NodeState, HashSet<Coordinate>> visitedCoordinatesByState = [];

        public bool AddVisited(PathNode path)
        {
            if (!visitedCoordinatesByState.TryGetValue(path.NodeState, out var visitedCoordinates))
            {
                visitedCoordinates = [];
                visitedCoordinatesByState[path.NodeState] = visitedCoordinates;
            }
            return visitedCoordinates.Add(path.Coordinate);
        }

        public bool HasVisited(PathNode path) => visitedCoordinatesByState.TryGetValue(path.NodeState, out var visitedCoordinate) && visitedCoordinate.Contains(path.Coordinate);
    }
}