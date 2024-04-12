namespace Pathfinder.Logic
{
    /// <summary>
    /// Defines logic for <see cref="Item"/> and holds/manages internal state.
    /// </summary>
    internal interface IItemLogic
    {
        decimal PickupCost { get; }
        bool InternalDataIsEqual(IItemLogic other);
        decimal GetDistanceFactor(Terrain terrain);
        int GetInternalDataHashCode();
    }
}