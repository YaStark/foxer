using foxer.Core.Game.Items;
using System;
using System.Collections.Generic;

namespace foxer.Core.Game.Craft
{
    public abstract class ItemCraftBase
    {
        public abstract IEnumerable<CraftRequirementsBase> GetRequirements();
        public abstract IReadOnlyDictionary<Type, int> GetResult();

        public abstract bool CanCraft(Stage stage);
        public abstract IEnumerable<ItemBase> Craft(Stage stage);
    }
}
