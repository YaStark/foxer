using foxer.Core.Game.Items;

namespace foxer.Core.Game.Entities
{
    public class HandTool : IToolItem
    {
        public int SwipeMs { get; } = 1000;

        public int HitMs => SwipeMs / 2;

        public WeaponKind WeaponKind => WeaponKind.Magic;

        public int Distance { get; } = 1;

        public bool CanInteract(EntityBase entity)
        {
            switch(entity)
            {
                case GrassEntity grass:
                    return grass.CanGather;

                case FlowerEntity flower:
                    return true;

                case TreeEntity tree:
                    return tree.Age < TreeEntity.AGE_MEDIUM;

                case StoneSmallEntity stone:
                    return true;

                default:
                    return false;
            }
        }

        public int GetSwipesCount(EntityBase entity)
        {
            if(entity is FlowerEntity flower)
            {
                return 1;
            }

            return 3;
        }
    }
}