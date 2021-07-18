using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;
using System;

namespace foxer.Core.Game
{
    public class LootManager
    {
        private static readonly Random _rnd = new Random();

        public ItemBase GetLoot(EntityBase entity)
        {
            switch(entity)
            {
                case TreeEntity tree: return GetTreeLoot(tree);
                case StoneSmallEntity stoneSmall: return GetRndLoot(i => new ItemStone(i), 1, 4);
                case StoneBigEntity stoneBig: return GetRndLoot(i => new ItemStone(i), 5, 8);
                default: return null;
            }
        }

        private ItemBase GetRndLoot<TItem>(Func<int, TItem> factory, int min, int max)
            where TItem : ItemBase
        {
            return factory(_rnd.Next(min, max));
        }

        private static ItemBase GetTreeLoot(TreeEntity tree)
        {
            const double maxCount = 15;
            if(tree.Age < TreeEntity.AGE_LARGE)
            {
                var stick = new ItemStick(1);
                stick.Count = (int) (tree.Age * 10 / TreeEntity.AGE_LARGE) + 2;
                return stick;
            }

            double value = Math.Min(1, tree.Age / TreeEntity.AGE_LARGE);
            return new ItemWood((int)(maxCount * value * (_rnd.NextDouble() / 4 + 0.75)));
        }
    }
}
