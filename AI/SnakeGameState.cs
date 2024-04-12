using System;
using System.Collections.ObjectModel;

namespace Snake.AI;

public class SnakeGameState
{
    public ReadOnlyCollection<Point> Snake { get; set; }
    public ReadOnlyCollection<Point> Apples { get; set; }
    public Direction Direction { get; set; }


    public SnakeGameState()
    {
        Snake ??= new(Array.Empty<Point>());
        Apples ??= new(Array.Empty<Point>());
    }
}
