using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinder.Logic
{
    /// <summary>
    /// Uses nodes representing runners moving across the map. Terminates when one runner has found the end or no runners are left.
    /// </summary>
    internal class RunnerBasedPathfinder : IPathfinder
    {
        private readonly Func<INodeStorage> nodeStorageFactory;
        private readonly INeighborNodeProvider neighborNodeProvider;
        private readonly IDistanceCalculator distanceCalculator;

        public event EventHandler<PathStepTakenEventArgs>? PathStepTaken;

        public RunnerBasedPathfinder(Func<INodeStorage> nodeStorageFactory, INeighborNodeProvider neighborNodeProvider, IDistanceCalculator distanceCalculator)
        {
            this.nodeStorageFactory = nodeStorageFactory;
            this.neighborNodeProvider = neighborNodeProvider;
            this.distanceCalculator = distanceCalculator;
        }

        public Path? FindPath(Board board)
        {
            if (board is null) throw new ArgumentNullException(nameof(board));
            if (board.Start is null)
                throw new ArgumentNullException(nameof(board));
            if (board.End is null)
                throw new ArgumentNullException(nameof(board));

            var visitedCoordinates = new StateDiscriminatingVisitedCoordinateStorage();
            var nodeStorage = nodeStorageFactory();
            PathNode? currentNode = new(board.Start.Value, null, new(Enumerable.Empty<Item>()));
            while (currentNode is not null && currentNode.Coordinate != board.End)
            {
                visitedCoordinates.AddVisited(currentNode);
                var currentNeighbors = GetNeighborNodes(board, visitedCoordinates, currentNode, neighborNodeProvider);
                AddNeighborsToStorage(nodeStorage, currentNeighbors, board);
                PathStepTaken?.Invoke(this, new PathStepTakenEventArgs { NewlyVisitedCoordinate = [currentNode.Coordinate], NewlyLivingCoordinates = [.. currentNeighbors.Select(n => n.Coordinate)] });
                currentNode = GetNextNode(nodeStorage, visitedCoordinates);
            }
            return currentNode?.ToPath(distanceCalculator, board);
        }

        private static PathNode? GetNextNode(INodeStorage nodeStorage, StateDiscriminatingVisitedCoordinateStorage visitedCoordinates)
        {
            PathNode? nextNode;
            do
            {
                nextNode = nodeStorage.PopNextNode();
                if (nextNode is null)
                    return null;
            } while (visitedCoordinates.HasVisited(nextNode));
            return nextNode;
        }

        private static void AddNeighborsToStorage(INodeStorage nodeStorage, IEnumerable<PathNode> neighborNodes, Board board)
        {
            foreach (var neighbor in neighborNodes)
                nodeStorage.AddNode(neighbor, board);
        }

        private static IEnumerable<PathNode> GetNeighborNodes(Board board, StateDiscriminatingVisitedCoordinateStorage visitedCoordinates, PathNode currentNode, INeighborNodeProvider neighborNodeProvider)
            => neighborNodeProvider.GetAccessibleNeighbors(board, currentNode)
                .Where(n => !visitedCoordinates.HasVisited(n));
    }
}