using foxer.Core.Game.Entities;
using System.Linq;

namespace foxer.Core.Game.Items
{
    public class ItemGrassWall : BuildableItemBase<GrassWallEntity>, IBuildableWall
    {
        public ItemGrassWall(int count)
        {
            Count = count;
        }

        public WallKind WallKind { get; set; }

        protected override EntityBase CreateEntity(int x0, int y0, int x, int y, bool preview)
        {
            var wall = new GrassWallEntity(x, y, WallKind);
            wall.Rotation = Rotation;
            return wall;
        }

        protected override bool OnCheckCanBuild(Stage stage, int x, int y, IPlatform platform)
        {
            return !stage.GetOverlappedEntites(x, y, platform.Level, 0.5f)
                .Any(e => e is IWall wall && wall.Rotation == Rotation);
        }
    }
}
