using foxer.Core.Game.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace foxer.Core.Game.Craft
{
    public class CraftManager
    {
    }

    public class PlayerCrafter : ICrafter
    {
        public IEnumerable<ICraftTask> ActiveQueue => throw new NotImplementedException();

        public IList<ItemBase> SourceInventory => throw new NotImplementedException();

        public IList<ItemBase> DestinationInventory => throw new NotImplementedException();

        public bool AddTask(ICraftTask task)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ItemCraftBase> AvailableCrafts(Stage stage)
        {
            throw new NotImplementedException();
        }

        public void RemoveTask(ICraftTask task)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ItemCraftBase> VisibleCrafts(Stage stage)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class ItemCraftBase
    {
        public abstract bool CanCraft(Stage stage);
        public abstract IEnumerable<ItemBase> Craft(Stage stage);
    }

    public class SimpleItemCraft<TItem> : ItemCraftBase
        where TItem : ItemBase, new()
    {
        private readonly int _count;
        private readonly CraftRequirementsBase[] _requirements;

        public SimpleItemCraft(int count, params CraftRequirementsBase[] requirements)
        {
            _count = count;
            _requirements = requirements;
        }

        public override bool CanCraft(Stage stage)
        {
            return _requirements.All(req => req.CanSpend(stage));
        }

        public override IEnumerable<ItemBase> Craft(Stage stage)
        {
            foreach (var req in _requirements)
            {
                req.Spend(stage);
            }

            return CreateItems();
        }

        private IEnumerable<ItemBase> CreateItems()
        {
            List<ItemBase> items = new List<ItemBase>();
            var item = new TItem();
            int stackSize = item.CanStack ? item.MaxStackSize : 1;
            int count = _count;
            while (count > 0)
            {
                item.Count = Math.Min(stackSize, count);
                count -= item.Count;
                items.Add(item);
                item = new TItem();
            }

            return items;
        }
    }

    public abstract class CraftRequirementsBase
    {
        public abstract bool CanSpend(Stage stage);
        public abstract void Spend(Stage stage);
    }

    public class PlayerCraftResourceRequirements<TItem> : CraftRequirementsBase
        where TItem : ItemBase
    {
        public int Count { get; }

        public PlayerCraftResourceRequirements(int count)
        {
            Count = count;
        }

        public override bool CanSpend(Stage stage)
        {
            return stage.InventoryManager.Contains<TItem>(Count);
        }

        public override void Spend(Stage stage)
        {
            throw new NotImplementedException();
        }
    }
}
