using foxer.Core.Game.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace foxer.Core.Game.Craft
{
    public class SimpleItemCraft<TItem> : ItemCraftBase
        where TItem : ItemBase
    {
        private readonly int _count;
        private readonly CraftRequirementsBase[] _requirements;
        private readonly Dictionary<Type, int> _result;

        public SimpleItemCraft(int count, params CraftRequirementsBase[] requirements)
        {
            _count = count;
            _requirements = requirements;
            _result = new Dictionary<Type, int>
            {
                [typeof(TItem)] = _count
            };
        }

        public override bool CanCraft(Stage stage)
        {
            return _requirements.All(req => req.Match(stage));
        }

        public override IEnumerable<ItemBase> Craft(Stage stage)
        {
            foreach (var req in _requirements)
            {
                req.Activate(stage);
            }

            return CreateItems(stage);
        }

        public override IEnumerable<CraftRequirementsBase> GetRequirements()
        {
            return _requirements;
        }

        public override IReadOnlyDictionary<Type, int> GetResult()
        {
            return _result;
        }

        private IEnumerable<ItemBase> CreateItems(Stage stage)
        {
            List<ItemBase> items = new List<ItemBase>();
            int stackSize = stage.ItemManager.MaxStackSize<TItem>();
            int count = _count;
            while (count > 0)
            {
                var item = stage.ItemManager.Create<TItem>(stage, Math.Min(stackSize, count));
                count -= item.Count;
                items.Add(item);
            }

            return items;
        }
    }
}
