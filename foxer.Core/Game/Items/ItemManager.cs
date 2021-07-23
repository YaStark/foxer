using System;
using System.Collections.Generic;

namespace foxer.Core.Game.Items
{
    public class ItemManager
    {
        private class ItemInfo
        {
            private readonly Func<Stage, int, ItemBase> _factory;

            public bool Stackable { get; }

            public ItemInfo(bool stackable, Func<Stage, int, ItemBase> factory)
            {
                Stackable = stackable;
                _factory = factory;
            }

            public ItemBase Create(Stage stage, int count)
            {
                return _factory.Invoke(stage, count);
            }
        }

        private static readonly Dictionary<Type, ItemInfo> _itemInfo = new Dictionary<Type, ItemInfo>();

        static ItemManager()
        {
            // resources
            AddItemInfo<ItemGrass>(true, (s, i) => new ItemGrass(i));
            AddItemInfo<ItemStick>(true, (s, i) => new ItemStick(i));
            AddItemInfo<ItemStone>(true, (s, i) => new ItemStone(i));
            AddItemInfo<ItemWood>(true, (s, i) => new ItemWood(i));

            // buldings
            AddItemInfo<ItemStoneOven>(false, (s, i) => new ItemStoneOven());
            AddItemInfo<ItemGrassWall>(true, (s, i) => new ItemGrassWall(i));
            AddItemInfo<ItemGrassFloor>(true, (s, i) => new ItemGrassFloor(i));

            // tools
            AddItemInfo<ItemDigStick>(false, (s, i) => new ItemDigStick());
            AddItemInfo<ItemStoneAxe>(false, (s, i) => new ItemStoneAxe());
        }

        public TItem Create<TItem>(Stage stage, int count)
            where TItem : ItemBase
        {
            return _itemInfo.TryGetValue(typeof(TItem), out var info)
                ? (TItem)info.Create(stage, count)
                : null;
        }

        public TItem Create<TItem>(Stage stage)
            where TItem : ItemBase
        {
            return _itemInfo.TryGetValue(typeof(TItem), out var info)
                ? (TItem)info.Create(stage, 1)
                : null;
        }

        public bool CanStack<TItem>()
        {
            return CanStack(typeof(TItem));
        }

        public bool CanStack(ItemBase item)
        {
            return CanStack(item.GetType());
        }

        public int MaxStackSize<TItem>()
        {
            return MaxStackSize(typeof(TItem));
        }

        public int MaxStackSize(ItemBase item)
        {
            return MaxStackSize(item.GetType());
        }

        public int MaxStackSize(Type itemType)
        {
            return CanStack(itemType) ? 37 : 1;
        }

        public bool CanStack(Type itemType)
        {
            return _itemInfo.TryGetValue(itemType, out var result) ? result.Stackable : false;
        }

        private static void AddItemInfo<TItem>(bool stackable, Func<Stage, int, ItemBase> factory)
            where TItem : ItemBase
        {
            _itemInfo[typeof(TItem)] = new ItemInfo(stackable, factory);
        }
    }
}
