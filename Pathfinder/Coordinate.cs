using System.Collections.Generic;

namespace Pathfinder.Logic
{
    public record struct Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public IEnumerable<Coordinate> GetOrthogonalNeighborCoordinates()
        {
            yield return this with { X = X + 1 };
            yield return this with { X = X - 1 };
            yield return this with { Y = Y + 1 };
            yield return this with { Y = Y - 1 };
        }

        public IEnumerable<Coordinate> GetDiagonalNeighborCoordinates()
        {
            yield return new(X + 1, Y + 1);
            yield return new(X + 1, Y - 1);
            yield return new(X - 1, Y + 1);
            yield return new(X - 1, Y - 1);
        }

        public IEnumerable<(Coordinate neighbor, Coordinate[] sharedNeighbors)> GetDiagonalNeighborCoordinatesWithSharedNeighbors()
        {
            yield return (new(X + 1, Y + 1), [this with { X = X + 1 }, this with { Y = Y + 1 }]);
            yield return (new(X + 1, Y - 1), [this with { X = X + 1 }, this with { Y = Y - 1 }]);
            yield return (new(X - 1, Y + 1), [this with { X = X - 1 }, this with { Y = Y + 1 }]);
            yield return (new(X - 1, Y - 1), [this with { X = X - 1 }, this with { Y = Y - 1 }]);
        }
    }
}