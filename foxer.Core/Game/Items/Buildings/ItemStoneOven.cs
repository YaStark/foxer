using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;

namespace foxer.Core.Game.Items
{
    public class ItemStoneOven : ItemBase, IBuildableItem
    {
        public bool CheckCanBuild(Stage stage, int x, int y)
        {
            return stage.GetCell(x, y)?.Kind == Cells.CellKind.Floor
                && stage.CanBeCreated<StoneOvenEntity>(x, y);
        }

        public EntityBase Create(Stage stage, int x, int y)
        {
            var oven = new StoneOvenEntity(x, y);
            oven.Rotation = GeomUtils.GetAngle90(stage.ActiveEntity.Cell, oven.Cell);
            return oven;
        }
        
        public bool CheckBuildDistance(Point player, Point target)
        {
            return MathUtils.L2(player, target) < 1.5;
        }

        public EntityBase CreatePreviewItem(int x0, int y0, int x, int y)
        {
            var oven = new StoneOvenEntity(x, y);
            oven.Rotation = GeomUtils.GetAngle90(new Point(x0, y0), oven.Cell);
            return oven;
        }
    }
}
