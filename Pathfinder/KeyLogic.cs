namespace Pathfinder.Logic
{
    internal class KeyLogic : IItemLogic
    {
        public decimal PickupCost => 0.5M;

        public ushort DoorValue { get; set; }

        public decimal GetDistanceFactor(Terrain terrain) => 1M;

        public int GetInternalDataHashCode() => DoorValue;

        public bool InternalDataIsEqual(IItemLogic other) => other is KeyLogic keyLogic && keyLogic.DoorValue == DoorValue;
    }
}