using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;

namespace foxer.Core.Game.Items
{
    public class ItemGrassFloor : ItemBase, IBuildableItem
    {
        public ItemGrassFloor(int count)
        {
            Count = count;
        }

        public bool CheckBuildDistance(Point player, Point target)
        {
            return MathUtils.L2(player, target) < 1.5;
        }

        public bool CheckCanBuild(Stage stage, int x, int y)
        {
            return stage.GetCell(x, y)?.Kind == Cells.CellKind.Floor
                && stage.CanBeCreated<GrassFloorEntity>(x, y);
        }

        public EntityBase Create(Stage stage, int x, int y)
        {
            var wall = new GrassFloorEntity(x, y);
            wall.Rotation = GeomUtils.GetAngle90(stage.ActiveEntity.Cell, wall.Cell);
            return wall;
        }

        public EntityBase CreatePreviewItem(int x0, int y0, int x, int y)
        {
            var wall = new GrassFloorEntity(x, y);
            wall.Rotation = GeomUtils.GetAngle90(new Point(x0, y0), wall.Cell);
            return wall;
        }
    }
}
