using foxer.Core.Game.Items;
using System.Collections.Generic;

namespace foxer.Core.Game.Craft
{
    public class PlayerHandsCrafter : CrafterBase
    {
        public PlayerHandsCrafter(Game game)
            : base(game.InventoryManager, game.InventoryManager)
        {
        }

        protected override IEnumerable<ItemCraftBase> GetAllCrafts()
        {
            yield return new SimpleItemCraft<ItemStoneAxe>(1,
                new CraftResourceRequirements<ItemStone>(this, 2));

            yield return new SimpleItemCraft<ItemDigStick>(1,
                new CraftResourceRequirements<ItemStone>(this, 1),
                new CraftResourceRequirements<ItemStick>(this, 1));

            yield return new SimpleItemCraft<ItemStoneOven>(1,
                new CraftResourceRequirements<ItemStone>(this, 15));
        }

        protected override void OnDropItems(Stage stage, ItemCraftBase recipe, List<ItemBase> items)
        {
            // todo on drop items
        }

        public override bool IsVisible(ItemCraftBase itemCraft)
        {
            // todo check is visible
            return base.IsVisible(itemCraft);
        }
    }
}
