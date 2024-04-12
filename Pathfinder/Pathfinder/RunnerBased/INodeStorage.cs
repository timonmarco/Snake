namespace Pathfinder.Logic
{
    internal interface INodeStorage
    {
        /// <summary>
        /// Returns and removes the next <see cref="PathNode"/> in the storage
        /// </summary>
        /// <returns>The next <see cref="PathNode"/>, or <see langword="null"/> if the <see cref="INodeStorage"/> is empty</returns>
        PathNode? PopNextNode();

        /// <summary>
        /// Adds <paramref name="node"/> to the <see cref="INodeStorage"/>
        /// </summary>
        /// <param name="node">The node to be added.</param>
        void AddNode(PathNode node, Board board);
    }
}