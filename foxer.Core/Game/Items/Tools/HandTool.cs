using foxer.Core.Game.Items;

namespace foxer.Core.Game.Entities
{
    public class HandTool : IToolItem
    {
        public int SwipeMs { get; } = 1000;

        public bool CanInteract(EntityBase entity)
        {
            switch(entity)
            {
                case GrassEntity grass:
                    return grass.CanGather;

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
            return 3;
        }
    }
}