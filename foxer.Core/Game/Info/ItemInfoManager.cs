using foxer.Core.Game.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace foxer.Core.Game.Info
{
    public class ItemInfoManager
    {
        private readonly Dictionary<Type, IItemInfoProvider> _itemsStrongTyped = new Dictionary<Type, IItemInfoProvider>();

        private readonly List<IItemInfoProviderWideTyped> _itemsWideTyped = new List<IItemInfoProviderWideTyped>();

        public ItemInfoManager()
        {
            _itemsWideTyped.Add(new CraftInfoProvider(this));
            _itemsWideTyped.Add(new RequirementsInfoProvider(this));

            // items
            Add(new ItemBaseInfoProvider<ItemGrass>("Grass", "Fresh and juicy"));
            Add(new ItemBaseInfoProvider<ItemStick>("Stick", "Thin and weak branch"));
            Add(new ItemBaseInfoProvider<ItemStone>("Stone", "Heavy enough"));
            Add(new ItemBaseInfoProvider<ItemWood>("Wood", "Solid and sturdy piece of wood"));

            Add(new ItemBaseInfoProvider<ItemStoneOven>("Stone oven", "Just 15 stones piled in a heap"));

            Add(new ItemBaseInfoProvider<ItemDigStick>("Digging stick", "To dig... a dirt?"));
            Add(new ItemBaseInfoProvider<ItemStoneAxe>("Stone axe", "Stone sharpened by another"));
        }

        private void Add<T>(ItemBaseInfoProvider<T> infoProvider)
            where T : ItemBase
        {
            _itemsStrongTyped[typeof(T)] = infoProvider;
        }

        private static string Mult(int count, string multiple, string single = null)
        {
            return (count > 1 ? multiple : single) ?? string.Empty;
        }

        public bool TryGetByObject<T>(T item, out IItemInfoProvider itemInfo)
        {
            if(TryGet(item.GetType(), out itemInfo))
            {
                return true;
            }

            itemInfo = _itemsWideTyped.FirstOrDefault(x => x.Match(item));
            return itemInfo != null;
        }

        public bool TryGet(Type itemType, out IItemInfoProvider itemInfo)
        {
            return _itemsStrongTyped.TryGetValue(itemType, out itemInfo);
        }
    }
}
