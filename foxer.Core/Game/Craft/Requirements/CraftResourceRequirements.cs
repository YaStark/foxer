using foxer.Core.Game.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace foxer.Core.Game.Craft
{
    public abstract class CraftResourceRequirementsBase : CraftRequirementsBase
    {
        private readonly Type[] _types;

        public int Count { get; }

        public CrafterBase Crafter { get; }

        public Type ItemType { get; }

        public CraftResourceRequirementsBase(CrafterBase crafter, Type itemType, int count, params Type[] sameTypes)
        {
            Count = count;
            Crafter = crafter;
            ItemType = itemType;
            _types = sameTypes;
        }

        public override bool Match(Stage stage)
        {
            int count = Count;
            foreach(var type in EnumerateMatchedTypes())
            {
                count -= Crafter.Source.Count(type);
                if (count <= 0) return true;
            }

            return false;
        }

        public override void Activate(Stage stage)
        {
            Crafter.Source.TrySpend(
                EnumerateMatchedTypes().ToArray(), 
                Count);
        }

        public IEnumerable<Type> EnumerateMatchedTypes()
        {
            yield return ItemType;
            foreach(var type in _types)
            {
                yield return type;
            }
        }
    }

    public class CraftResourceRequirements<TItem> : CraftResourceRequirementsBase
        where TItem : ItemBase
    {
        public CraftResourceRequirements(CrafterBase crafter, int count) 
            : base(crafter, typeof(TItem), count)
        {
        }
    }
}
