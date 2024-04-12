namespace Snake.AI;

public record struct Point(int X, int Y)
{
    public readonly bool IsNeighbor(Point b) =>
        (b.X == X + 1 || b.X == X - 1) && b.Y == Y ||
        (b.Y == Y + 1 || b.Y == Y - 1) && b.X == X;
}
