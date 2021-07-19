using foxer.Core.Game.Inventory;
using foxer.Core.Game.Items;
using System.Collections.Generic;
using System.Linq;

namespace foxer.Core.Game.Craft
{
    public abstract class CrafterBase
    {
        private IReadOnlyList<ItemCraftBase> _allCrafts = null;

        public InventoryManagerBase Source { get; }

        public InventoryManagerBase Destination { get; }

        public IReadOnlyList<ItemCraftBase> Crafts
        {
            get
            {
                return _allCrafts == null
                    ? _allCrafts = GetAllCrafts().ToArray()
                    : _allCrafts;
            }
        }

        protected CrafterBase(
            InventoryManagerBase source, 
            InventoryManagerBase destination)
        {
            Source = source;
            Destination = destination;
        }

        public bool Craft(Stage stage, ItemCraftBase recipe)
        {
            if (!recipe.CanCraft(stage)
                || !CheckCanCraft(stage, recipe))
            {
                return false;
            }

            return OnCraft(stage, recipe);
        }

        protected virtual bool CheckCanCraft(Stage stage, ItemCraftBase recipe)
        {
            return recipe.GetResult().All(kv => Destination.CanAccomodate(kv.Key, kv.Value));
        }

        protected virtual bool OnCraft(Stage stage, ItemCraftBase recipe)
        {
            var itemsToDrop = new List<ItemBase>();
            foreach (var item in recipe.Craft(stage))
            {
                if (!Destination.Accomodate(item))
                {
                    itemsToDrop.Add(item);
                }
            }

            OnDropItems(stage, recipe, itemsToDrop);
            return true;
        }

        public virtual bool IsVisible(ItemCraftBase itemCraft)
        {
            return true;
        }

        public virtual IReadOnlyList<ItemCraftBase> GetVisibleCrafts(Stage stage)
        {
            return Crafts.Where(IsVisible).ToArray();
        }

        protected abstract void OnDropItems(Stage stage, ItemCraftBase recipe, List<ItemBase> items);

        protected abstract IEnumerable<ItemCraftBase> GetAllCrafts();
    }
}
