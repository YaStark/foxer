using foxer.Core.Game.Craft;
using System;
using System.Linq;

namespace foxer.Core.Game.Info
{
    public class CraftInfoProvider : IItemInfoProviderWideTyped
    {
        private readonly ItemInfoManager _itemInfoManager;

        public CraftInfoProvider(ItemInfoManager itemInfoManager)
        {
            _itemInfoManager = itemInfoManager;
        }

        public bool Match<T>(T item)
        {
            return item is ItemCraftBase;
        }

        public string GetDescription(object item, Stage stage)
        {
            if(item is ItemCraftBase craft
                && !craft.CanCraft(stage))
            {
                return "Not enough resources";
            }

            return string.Empty;
        }

        public string GetName(object item, Stage stage)
        {
            if (!(item is ItemCraftBase craft))
            {
                return string.Empty;
            }

            var items = string.Join("; ", craft.GetResult()
                .Select(kv => CreateResultDescription(stage, kv.Key, kv.Value)));
            return "Craft " + items;
        }

        private string CreateResultDescription(Stage stage, Type type, int count)
        {
            string name = _itemInfoManager.TryGet(type, out var itemInfo)
                ? itemInfo.GetName(null, stage) : "???";
            return $"{name} ({count})";
        }
    }
}
