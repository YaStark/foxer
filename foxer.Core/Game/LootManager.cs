using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;
using System;

namespace foxer.Core.Game
{
    public class LootManager
    {
        private static readonly Random _rnd = new Random();

        public ItemBase GetLoot(Stage stage, EntityBase entity)
        {
            switch(entity)
            {
                case TreeEntity tree: return GetTreeLoot(stage, tree);
                case StoneSmallEntity stoneSmall: return GetRndLoot<ItemStone>(stage, 1, 4);
                case StoneBigEntity stoneBig: return GetRndLoot<ItemStone>(stage, 5, 8);
                case GrassEntity grass: return GetRndLoot<ItemGrass>(stage, 5, 8); 
                default: return null;
            }
        }

        private ItemBase GetRndLoot<TItem>(Stage stage, int min, int max)
            where TItem : ItemBase
        {
            return stage.ItemManager.Create<TItem>(stage, _rnd.Next(min, max));
        }

        private static ItemBase GetTreeLoot(Stage stage, TreeEntity tree)
        {
            const double maxCount = 15;
            if(tree.Age < TreeEntity.AGE_LARGE)
            {
                var stick = stage.ItemManager.Create<ItemStick>(stage);
                stick.Count = (int) (tree.Age * 10 / TreeEntity.AGE_LARGE) + 2;
                return stick;
            }

            double value = Math.Min(1, tree.Age / TreeEntity.AGE_LARGE);
            int count = (int)(maxCount * value * (_rnd.NextDouble() / 4 + 0.75));
            return stage.ItemManager.Create<ItemWood>(stage, count);
        }
    }
}
