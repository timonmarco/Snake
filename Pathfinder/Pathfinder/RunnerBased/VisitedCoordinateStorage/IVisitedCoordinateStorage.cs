namespace Pathfinder.Logic
{
    internal interface IVisitedCoordinateStorage
    {
        bool HasVisited(PathNode path);
        bool AddVisited(PathNode path);
    }
}