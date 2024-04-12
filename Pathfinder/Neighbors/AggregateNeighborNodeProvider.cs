using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinder.Logic
{
    internal class AggregateNeighborNodeProvider : INeighborNodeProvider
    {
        private readonly INeighborNodeProvider[] neighborNodeProviders;

        public AggregateNeighborNodeProvider(IEnumerable<INeighborNodeProvider> neighborNodeProviders)
            : this(neighborNodeProviders.ToArray())
        {
        }

        public AggregateNeighborNodeProvider(params INeighborNodeProvider[] neighborNodeProviders)
        {
            this.neighborNodeProviders = neighborNodeProviders ?? throw new ArgumentNullException(nameof(neighborNodeProviders));
            if (neighborNodeProviders.Length == 0)
                throw new ArgumentException($"Requires at least one {nameof(INeighborNodeProvider)}");
            if (neighborNodeProviders.Any(p => p is null))
                throw new ArgumentException("At least one provider was null.");
        }

        public IEnumerable<PathNode> GetAccessibleNeighbors(Board board, PathNode node) => neighborNodeProviders.SelectMany(p => p.GetAccessibleNeighbors(board, node));
    }
}