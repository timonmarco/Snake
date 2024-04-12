using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Snake.AI;

internal class PathCandidates
{
    private readonly ConcurrentQueue<Task<PriorityPath>> pathCandidates = [];
    private readonly int totalTaskCount;
    private readonly int desiredPriority;
    private int completedTaskCount = 0;
    private bool reachedDesiredPriority;

    public PathCandidates((Task<Point[]?> pathTask, int priority)[] pathCandidateTasks)
    {
        totalTaskCount = pathCandidateTasks.Length;
        foreach (var task in pathCandidateTasks.OrderByDescending(pair => pair.priority).Select(async pair => new PriorityPath(await pair.pathTask, pair.priority)))
            pathCandidates.Enqueue(task);
    }

    public (bool RanToCompletion, PriorityPath BestPath) GetPathInfo()
    {
        PriorityPath emptyPath = new(null, int.MinValue);
        if (!pathCandidates.TryPeek(out var headTask))
            return (false, emptyPath);
        if (headTask.IsFaulted)
        {
            _ = pathCandidates.TryDequeue(out _);
            return GetPathInfo();
        }
        if (!headTask.IsCompleted)
            return (false, emptyPath);
        if (headTask.Result is not PriorityPath headPath || headPath.Path is null)
        {
            _ = pathCandidates.TryDequeue(out _);
            return GetPathInfo();
        }
        return (true, headPath);
    }
}
