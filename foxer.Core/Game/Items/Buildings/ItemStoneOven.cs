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
            oven.Rotation = GeomUtils.GetAngle(stage.ActiveEntity.Cell, oven.Cell);
            return oven;
        }
        
        public bool CheckBuildDistance(Point player, Point target)
        {
            return MathUtils.L2(player, target) < 3;
        }
    }
}
