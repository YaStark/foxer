using foxer.Core.Game.Entities;

namespace foxer.Core.Game.Items
{
    public class ItemGrassRoof : BuildableItemBase<GrassRoofEntity>
    {
        public ItemGrassRoof(int count)
        {
            Count = count;
        }

        public RoofKind RoofKind { get; set; }

        protected override EntityBase CreateEntity(int x0, int y0, int x, int y, bool preview)
        {
            var item = new GrassRoofEntity(x, y, 0);
            item.Rotation = Rotation;
            item.RoofKind = RoofKind;
            return item;
        }
    }
}
