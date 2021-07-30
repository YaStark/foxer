using foxer.Core.Game.Entities;

namespace foxer.Core.Game.Items
{
    public class ItemDigStick : ItemBase, IToolItem
    {
        public int SwipeMs => 800;

        public WeaponKind WeaponKind => WeaponKind.Spear;

        public bool CanInteract(EntityBase entity)
        {
            if(entity is IConstruction construction
                && construction.ConstructionLevel == ConstructionLevel.Primitive)
            {
                return true;
            }

            // todo dig
            return false;
        }

        public int GetSwipesCount(EntityBase entity)
        {
            if (entity is IConstruction)
            {
                return 2;
            }

            return 4;
        }
    }
}
