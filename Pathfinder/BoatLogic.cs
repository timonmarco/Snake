namespace Pathfinder.Logic
{
    internal class BoatLogic : IItemLogic
    {
        public decimal PickupCost => 4M;

        public decimal GetDistanceFactor(Terrain terrain) => terrain switch
        {
            Terrain.Water => 0.5M,
            _ => 2M
        };

        public int GetInternalDataHashCode() => 0;

        public bool InternalDataIsEqual(IItemLogic other) => other is BoatLogic;
    }
}