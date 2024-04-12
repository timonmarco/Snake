using System;

namespace Pathfinder.Logic
{
    public interface IPathfinder
    {
        event EventHandler<PathStepTakenEventArgs>? PathStepTaken;
        Path? FindPath(Board board);
    }
}