using foxer.Core.Game.Entities;

namespace foxer.Core.Game.Items
{
    public class ItemGrassFloor : BuildableItemBase<GrassFloorEntity>
    {
        public ItemGrassFloor(int count)
        {
            Count = count;
        }

        protected override EntityBase CreateEntity(int x0, int y0, int x, int y, bool preview)
        {
            return new GrassFloorEntity(x, y, 0);
        }

        public override bool UseRotation()
        {
            return false;
        }
    }
}
