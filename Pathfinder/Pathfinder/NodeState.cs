using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinder.Logic
{
    internal class NodeState : IEquatable<NodeState>
    {
        private readonly HashSet<Item> items;
        private readonly Lazy<int> hashCode;

        public NodeState(IEnumerable<Item> items)
        {
            this.items = items?.ToHashSet() ?? throw new ArgumentNullException(nameof(items));
            hashCode = new(GenerateHashCode);
        }

        public Item[] GetItems() => items.ToArray();

        public bool HasItem(Item item) => items.Contains(item);

        public bool Equals(NodeState? other)
        {
            if (other is null)
                return false;
            return items.SetEquals(other.items);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as NodeState);
        }

        public override int GetHashCode()
        {
            return hashCode.Value;
        }

        private int GenerateHashCode()
        {
            var hashCode = new HashCode();
            foreach (var itemHash in items.Select(i => i.GetHashCode()).OrderBy(x => x))
                hashCode.Add(itemHash);
            return hashCode.ToHashCode();
        }
    }
}