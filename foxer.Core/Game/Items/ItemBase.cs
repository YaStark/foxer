using System;

namespace foxer.Core.Game.Items
{
    public abstract class ItemBase
    {
        public bool CanStack { get; protected set; }

        public int MaxStackSize { get; protected set; }

        public int Count { get; set; }

        public ItemKind Kind { get; }

        protected ItemBase(ItemKind kind)
        {
            Kind = kind;
        }
    }

    public abstract class StackableItemBase : ItemBase
    {
        protected StackableItemBase(ItemKind kind, int stackCount)
            : base(kind)
        {
            CanStack = true;
            MaxStackSize = stackCount;
        }
    }

    public abstract class UnstackableItemBase : ItemBase
    {
        protected UnstackableItemBase(ItemKind kind)
            : base(kind)
        {
            CanStack = false;
            MaxStackSize = 1;
        }
    }
}
