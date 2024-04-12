namespace Pathfinder.Logic
{
    internal interface INodeMetric
    {
        decimal GetValue(PathNode node, Board board);
    }
}