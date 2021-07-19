using foxer.Core.Game.Entities;

namespace foxer.Core.Game.Items
{
    public class ItemDigStick : ItemBase, IToolItem
    {
        public int SwipeMs => 800;

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
