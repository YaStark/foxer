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
            // always visible

            yield return new SimpleItemCraft<ItemStoneAxe>(1,
                new CraftResourceRequirements<ItemStone>(this, 2));

            yield return new SimpleItemCraft<ItemDigStick>(1,
                new CraftResourceRequirements<ItemStone>(this, 1),
                new CraftResourceRequirements<ItemStick>(this, 1));

            // after get stones

            yield return new SimpleItemCraft<ItemStoneOven>(1,
                new CraftResourceRequirements<ItemStone>(this, 15));

            // after get grass

            yield return new SimpleItemCraft<ItemGrassFloor>(1,
                new CraftResourceRequirements<ItemStick>(this, 8),
                new CraftResourceRequirements<ItemGrass>(this, 8));

            yield return new SimpleItemCraft<ItemGrassWall>(1,
                new CraftResourceRequirements<ItemStick>(this, 4),
                new CraftResourceRequirements<ItemGrass>(this, 4));

            yield return new SimpleItemCraft<ItemGrassRoof>(1,
                new CraftResourceRequirements<ItemStick>(this, 4),
                new CraftResourceRequirements<ItemGrass>(this, 4));

            // after get flowers

            yield return new SimpleItemCraft<ItemGrass>(1,
                new FlowersRequirements(this, 2));

            yield return new SimpleItemCraft<ItemDandelion>(1,
                new CraftResourceRequirements<ItemGrass>(this, 5));
            yield return new SimpleItemCraft<ItemRedFlower>(1,
                new CraftResourceRequirements<ItemGrass>(this, 5));
            yield return new SimpleItemCraft<ItemSunflower>(1,
                new CraftResourceRequirements<ItemGrass>(this, 5));
            yield return new SimpleItemCraft<ItemBlueFlower>(1,
                new CraftResourceRequirements<ItemGrass>(this, 5));
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
