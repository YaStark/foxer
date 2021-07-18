using foxer.Core.Game.Entities;

namespace foxer.Core.Game.Items
{
    public class ItemDigStick : UnstackableItemBase, IToolItem
    {
        public int SwipeMs => 800;

        public ItemDigStick()
            : base(ItemKind.Shovel)
        {
        }

        public bool CanInteract(EntityBase entity)
        {
            return false; // todo
        }

        public int GetSwipesCount(EntityBase entity)
        {
            return 4;
        }
    }
}
