using System;
using System.Linq;
using Pathfinder.Logic;

namespace Snake.AI;

internal class SnakePathfinder
{
    private readonly Board pathfinderBoard;
    private readonly bool invertHeightAndWidth;

    public SnakePathfinder(ISnakeGame game)
    {
        invertHeightAndWidth = game.Width > 5 * game.Height;
        pathfinderBoard = new(invertHeightAndWidth ? game.Height : game.Width, invertHeightAndWidth ? game.Width : game.Height);
    }

    public void FillFromGameState(SnakeGameState gameState)
    {
        foreach (var bodyPart in gameState.Snake)
            pathfinderBoard.SetTerrain(ToPathfinderCoordinate(bodyPart), Terrain.Obstacle);
        if (gameState.Snake.Count == 1)
        {
            var behindHead = gameState.Direction switch
            {
                Direction.Up => gameState.Snake[0] with { Y = gameState.Snake[0].Y + 1 },
                Direction.Down => gameState.Snake[0] with { Y = gameState.Snake[0].Y - 1 },
                Direction.Left => gameState.Snake[0] with { X = gameState.Snake[0].X + 1 },
                Direction.Right => gameState.Snake[0] with { X = gameState.Snake[0].X - 1 },
                _ => throw new NotImplementedException(),
            };
            pathfinderBoard.SetTerrain(ToPathfinderCoordinate(behindHead), Terrain.Obstacle);
        }
        pathfinderBoard.Start = ToPathfinderCoordinate(gameState.Snake[0]);
        var end = gameState.Apples.Count > 0 ?
            gameState.Apples[0] :
            gameState.Snake[gameState.Snake.Count - 1];
        pathfinderBoard.End = ToPathfinderCoordinate(end);
    }

    public void SetEnd(Point end) => pathfinderBoard.End = ToPathfinderCoordinate(end);

    public void FillFromSnake(Point[] snakePoints)
    {
        foreach (var bodyPart in snakePoints.Select(ToPathfinderCoordinate))
            pathfinderBoard.SetTerrain(bodyPart, Terrain.Obstacle);
        pathfinderBoard.Start = ToPathfinderCoordinate(snakePoints[0]);
        pathfinderBoard.End = ToPathfinderCoordinate(snakePoints[snakePoints.Length - 1]);
    }

    public Point[]? FindPath(PathfinderKind pathfinderKind) => PathfinderFactory.CreatePathfinder(pathfinderKind, NeighborGenerationKind.Orthogonal, DistanceMode.Simple)
        .FindPath(pathfinderBoard)
        ?.PathCoordinates
        .Select(ToSnakePoint)
        .ToArray();

    private Coordinate ToPathfinderCoordinate(Point snakePoint) => invertHeightAndWidth ?
        new(snakePoint.Y, snakePoint.X) :
        new(snakePoint.X, snakePoint.Y);

    private Point ToSnakePoint(Coordinate coordinate) => invertHeightAndWidth ?
        new(coordinate.Y, coordinate.X) :
        new(coordinate.X, coordinate.Y);
}