using System;
using System.Collections.Generic;

namespace Pathfinder.Logic
{
    public readonly struct Item : IEquatable<Item>
    {
        private readonly Guid typeId;
        private readonly IItemLogic itemLogic;
        private static readonly Dictionary<ushort, Item> keys = [];
        private static readonly Item boat = new(new Guid("6e4ad488-5673-4a01-8aaa-e5b4cb00c5c0"), new BoatLogic());

        public readonly decimal PickupCost => itemLogic.PickupCost;

        private Item(Guid typeId, IItemLogic itemLogic)
        {
            this.typeId = typeId;
            this.itemLogic = itemLogic ?? throw new ArgumentNullException(nameof(itemLogic));
        }

        public static Item GetBoat() => boat;

        public static Item GetKey(ushort doorValue)
        {
            if (keys.TryGetValue(doorValue, out var key))
                return key;
            key = new(new Guid("93d4aceb-800b-4a85-8964-1f6374ba3291"), new KeyLogic { DoorValue = doorValue });
            keys.Add(doorValue, key);
            return key;
        }

        public readonly decimal GetDistanceFactor(Terrain terrain) => itemLogic.GetDistanceFactor(terrain);

        public override bool Equals(object? obj)
        {
            if (obj is not Item otherItem)
                return false;
            return Equals(otherItem);
        }

        public bool Equals(Item other) => typeId == other.typeId && itemLogic.InternalDataIsEqual(other.itemLogic);

        public override readonly int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(typeId);
            hash.Add((uint)itemLogic.GetInternalDataHashCode());
            return hash.ToHashCode();
        }

        public static bool operator ==(Item left, Item right) => left.Equals(right);

        public static bool operator !=(Item left, Item right) => !(left == right);
    }
}