using System;
using System.Collections.Generic;

namespace Pathfinder.Logic
{
    public class PathfinderFactory
    {
        public static IPathfinder CreatePathfinder(PathfinderKind pathfinderKind, NeighborGenerationKind neighborGenerationKind, DistanceMode distanceMode)
        {
            var neighborNodeProvider = CreateNeighborNodeProvider(neighborGenerationKind);
            var distanceCalculator = CreateDistanceCalculator(neighborGenerationKind, distanceMode);
            var nodeStorageFactory = CreateNodeStorageFactory(pathfinderKind, distanceCalculator);
            return new RunnerBasedPathfinder(nodeStorageFactory, neighborNodeProvider, distanceCalculator);
        }

        private static Func<INodeStorage> CreateNodeStorageFactory(PathfinderKind pathfinderKind, IDistanceCalculator distanceCalculator)
        {
            return pathfinderKind switch
            {
                PathfinderKind.BreadthFirst => () => new BreadthFirstNodeStorage(),
                PathfinderKind.GreedyBestFirst => () => new BestFirstNodeStorage(new GreedyBestFirstMetric(distanceCalculator)),
                PathfinderKind.AStar => () => new BestFirstNodeStorage(new AStarMetric(distanceCalculator)),
                PathfinderKind.DepthFirst => () => new DepthFirstNodeStorage(),
                PathfinderKind.FakeBreadthFirst => () => new BestFirstNodeStorage(new BreadthFirstMetric(distanceCalculator)),
                PathfinderKind.AntiAStar => () => new BestFirstNodeStorage(new AntiAStarMetric(distanceCalculator)),
                _ => throw new NotImplementedException(),
            };
        }

        private static IDistanceCalculator CreateDistanceCalculator(NeighborGenerationKind neighborGenerationKind, DistanceMode distanceMode) => (distanceMode, neighborGenerationKind) switch
        {
            (_, NeighborGenerationKind.Orthogonal) => new SimpleOrthogonalDistanceCalculator(),
            (DistanceMode.Geometric, _) => new GeometricDistanceCalculator(),
            (DistanceMode.Simple, _) => new SimpleOmnidirectionalDistanceCalculator(),
            _ => throw new NotImplementedException()
        };

        private static DoorNeighborNodeFilter CreateNeighborNodeProvider(NeighborGenerationKind neighborGenerationKind)
        {
            var neighborProviders = new List<INeighborNodeProvider> { new OrthogonalNeighborNodeProvider(), new TeleporterNeighborNodeProvider(), new ItemPickupNeighborNodeProvider(), new ItemDropNeighborNodeProvider() };
            if (neighborGenerationKind is NeighborGenerationKind.Omnidirectional)
                neighborProviders.Add(new DiagonalNeighborNodeProvider());
            else if (neighborGenerationKind is NeighborGenerationKind.OmnidirectionalGhost)
                neighborProviders.Add(new DiagonalGhostNeighborNodeProvider());
            return new DoorNeighborNodeFilter(new AggregateNeighborNodeProvider(neighborProviders));
        }
    }
}
