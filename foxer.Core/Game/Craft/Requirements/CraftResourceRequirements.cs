using foxer.Core.Game.Items;
using System;

namespace foxer.Core.Game.Craft
{
    public abstract class CraftResourceRequirementsBase : CraftRequirementsBase
    {
        public int Count { get; }

        public CrafterBase Crafter { get; }

        public Type ItemType { get; }

        public CraftResourceRequirementsBase(CrafterBase crafter, Type itemType, int count)
        {
            Count = count;
            Crafter = crafter;
            ItemType = itemType;
        }

        public override bool Match(Stage stage)
        {
            return Crafter.Source.Contains(ItemType, Count);
        }

        public override void Activate(Stage stage)
        {
            Crafter.Source.TrySpend(ItemType, Count);
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
