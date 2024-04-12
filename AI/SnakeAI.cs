using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pathfinder.Logic;

namespace Snake.AI;

/// <summary>
/// Can take control of a <see cref="SnakeGame"/> instance
/// </summary>
public partial class SnakeAI
{
    public async Task ControlSnakeAsync(ISnakeGame game)
    {
        while (true)
            await ControlSnakeInternalAsync(game).ConfigureAwait(false);
    }

    private static Task ControlSnakeInternalAsync(ISnakeGame game)
    {
        FollowPath(game);
        return Task.CompletedTask;
    }

    private static PathCandidates GetPathCandidates(ISnakeGame game)
    {
        var gameState = game.GameState;
        var tasks = new List<(Task<Point[]?>, int)>();
        if (gameState.Snake.Count >= game.Width * game.Height * 2 / 3)
            tasks.Add((Task.Run(() => GetSaveApplePath(game, gameState, PathfinderKind.AntiAStar)), 4));
        tasks.AddRange([
            (Task.Run(() => GetSaveApplePath(game, gameState, PathfinderKind.AStar)), 3),
            (Task.Run(() => GetTailPath(game, gameState, PathfinderKind.AntiAStar)), -1),
            (Task.Run(() => GetTailPath(game, gameState, PathfinderKind.AStar)), -2),
        ]);
        return new([.. tasks]);
    }

    private static Point[]? GetTailPath(ISnakeGame game, SnakeGameState gameState, PathfinderKind pathfinderKind)
    {
        var pathfinder = new SnakePathfinder(game);
        pathfinder.FillFromGameState(gameState);
        pathfinder.SetEnd(gameState.Snake[gameState.Snake.Count - 1]);
        return pathfinder.FindPath(pathfinderKind);
    }

    private static Point[]? GetSaveApplePath(ISnakeGame game, SnakeGameState gameState, PathfinderKind pathfinderKind)
    {
        var pathfinder = new SnakePathfinder(game);
        pathfinder.FillFromGameState(gameState);
        var path = pathfinder.FindPath(pathfinderKind);
        if (path != null && CanFollowTailPostPath(game, gameState, path))
            return path;
        return null;
    }

    private static void FollowPath(ISnakeGame game)
    {
        var lastHandledCoordinateIndex = -1;
        var lastCalculatedCoordinateIndex = 0;
        var pathCandidates = GetPathCandidates(game);
        PriorityPath? path = null;
        while (path?.Path is null || TryTakeStep(path.Path, game, ref lastHandledCoordinateIndex))
        {
            if (lastHandledCoordinateIndex > lastCalculatedCoordinateIndex) // We took a step and the path should therefore be recalculated
            {
                pathCandidates = GetPathCandidates(game);
                lastCalculatedCoordinateIndex = lastHandledCoordinateIndex;
            }
            else
            {
                var (ranToCompletion, pathCandidate) = pathCandidates.GetPathInfo();
                if (pathCandidate?.Path is null && path is null && ranToCompletion)
                    break;
                if (PathCandidateIsBetterThanPath(path, pathCandidate))
                {
                    path = pathCandidate;
                    lastHandledCoordinateIndex = -1;
                    lastCalculatedCoordinateIndex = 0;
                }
            }
        }
    }

    private static bool PathCandidateIsBetterThanPath(PriorityPath? path, PriorityPath? pathCandidate)
    {
        if (path == pathCandidate)
            return false;
        if (pathCandidate?.Path is null)
            return false;
        if (pathCandidate.Path.Length <= 1)
            return false;
        if (path?.Path is null)
            return true;
        return pathCandidate.Priority >= path.Priority;
    }

    private static bool TryTakeStep(Point[] path, ISnakeGame game, ref int lastHandledCoordinateIndex)
    {
        var gameState = game.GameState;
        var snakeHead = gameState.Snake[0];
        if (lastHandledCoordinateIndex >= 0)
        {
            var lastHandledCoordinate = path[lastHandledCoordinateIndex];
            if (lastHandledCoordinate == snakeHead)
                return true;
        }
        var currentCoordinate = snakeHead;
        var currentCoordinateIndex = Array.IndexOf(path, currentCoordinate, Math.Max(0, lastHandledCoordinateIndex));
        if (currentCoordinateIndex == -1 || currentCoordinateIndex >= path.Length - 1)
            return false;
        if (lastHandledCoordinateIndex == currentCoordinateIndex)
            return true;
        var nextCoordinate = path[currentCoordinateIndex + 1];
        var yDiff = nextCoordinate.Y - currentCoordinate.Y;
        var xDiff = nextCoordinate.X - currentCoordinate.X;
        Direction direction;
        if (yDiff == 1)
            direction = Direction.Down;
        else if (yDiff == -1)
            direction = Direction.Up;
        else if (xDiff == 1)
            direction = Direction.Right;
        else if (xDiff == -1)
            direction = Direction.Left;
        else
            return false;
        lastHandledCoordinateIndex = currentCoordinateIndex;
        if (gameState.Direction == direction)
            return true;
        var oppositeDirection = direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => gameState.Direction,
        };
        if (gameState.Direction == oppositeDirection)
            return false;
        game.SetDirection(direction);
        return true;
    }

    private static bool CanFollowTailPostPath(ISnakeGame game, SnakeGameState gameState, Point[] path)
    {
        if (path is null)
            return false;
        var pathfinder = new SnakePathfinder(game);
        var unchangedBodyCount = Math.Max(0, gameState.Snake.Count + 1 - path.Length);
        var newBodyCount = Math.Min(path.Length, gameState.Snake.Count + 1);
        var postPathSnake = path.Reverse().Take(newBodyCount)
            .Concat(gameState.Snake.Skip(1).Take(unchangedBodyCount))
            .ToArray();
        pathfinder.FillFromSnake(postPathSnake);
        return pathfinder.FindPath(PathfinderKind.GreedyBestFirst) != null;
    }
}
