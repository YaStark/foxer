using foxer.Core.Game.Entities;

namespace foxer.Core.Game.Items
{
    public class ItemDigStick : ItemBase, IWeaponItem
    {
        public int SwipeMs { get; } = 800;
        
        public int HitMs => SwipeMs / 2;

        public WeaponKind WeaponKind { get; } = WeaponKind.Spear;

        public int ToolDistance { get; } = 1;

        public bool CanInteract(EntityBase entity)
        {
            if(entity is IConstruction construction
                && construction.ConstructionLevel == ConstructionLevel.Primitive)
            {
                return true;
            }

            if(entity.AttackTarget != null)
            {
                return true;
            }

            return false;
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
