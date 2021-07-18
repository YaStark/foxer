﻿using foxer.Core.Game.Entities;

namespace foxer.Core.Game.Items
{
    public class ItemStoneAxe : UnstackableItemBase, IToolItem
    {
        public int SwipeMs => 800;

        public ItemStoneAxe()
            : base(ItemKind.Axe)
        {
        }

        public bool CanInteract(EntityBase entity)
        {
            return entity is TreeEntity tree
                && tree.Age < TreeEntity.AGE_LARGE;
        }

        public int GetSwipesCount(EntityBase entity)
        {
            if(entity is TreeEntity tree)
            {
                return tree.Age < TreeEntity.AGE_MEDIUM ? 2 : 4;
            }

            return -1;
        }
    }
}
