using foxer.Core.Game.Entities;
using System.Linq;

namespace foxer.Core.Game.Items
{
    public class ItemGrassWall : BuildableItemBase<GrassWallEntity>
    {
        public ItemGrassWall(int count)
        {
            Count = count;
        }

        protected override EntityBase CreateEntity(int x0, int y0, int x, int y, bool preview)
        {
            var wall = new GrassWallEntity(x, y, 0);
            wall.Rotation = Rotation;
            return wall;
        }
    }
}
