using foxer.Core.Game.Entities;

namespace foxer.Core.Game.Items
{
    public class ItemStoneOven : BuildableItemBase<StoneOvenEntity>
    {
        protected override EntityBase CreateEntity(int x0, int y0, int x, int y, bool preview)
        {
            var oven = new StoneOvenEntity(x, y, 0);
            oven.Rotation = Rotation;
            return oven;
        }
    }
}
