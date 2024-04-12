namespace Pathfinder.Logic
{
    internal interface IDistanceCalculator
    {
        decimal GetMinimumDistance(Coordinate a, Coordinate b, Board board);
        decimal GetDistanceToNeighbor(PathNode node, PathNode neighbor, Board board);
    }
}