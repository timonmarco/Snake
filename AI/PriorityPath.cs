namespace Snake.AI;

internal record class PriorityPath
{
    public Point[] Path { get; }
    public int Priority { get; }

    public PriorityPath(Point[]? path, int priority)
    {
        Path = path;
        Priority = priority;
    }
}
