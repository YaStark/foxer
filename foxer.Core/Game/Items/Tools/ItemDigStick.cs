using foxer.Core.Game.Entities;

namespace foxer.Core.Game.Items
{
    public class ItemDigStick : ItemBase, IToolItem, IWeaponItem
    {
        public int SwipeMs { get; } = 800;
        
        public int HitMs => SwipeMs / 2;

        public WeaponKind WeaponKind { get; } = WeaponKind.Spear;

        public int Distance { get; } = 1;

        public bool CanInteract(EntityBase entity)
        {
            if(entity is IConstruction construction
                && construction.ConstructionLevel == ConstructionLevel.Primitive)
            {
                return true;
            }

            return false;
        }

        public bool CanInteract(EntityFighterBase entity)
        {
            return true;
        }

        public int GetDamage(Stage stage, EntityBase target)
        {
            return 3; // todo
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
