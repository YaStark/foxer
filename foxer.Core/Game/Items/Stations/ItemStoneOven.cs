using System;
using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Info;
using foxer.Core.Utils;

namespace foxer.Core.Game.Items
{
    public class ItemStoneOven : ItemBase, IBuildableItem
    {
        private StoneOvenEntity _oven = new StoneOvenEntity(0, 0);

        public bool CheckCanBuild(Stage stage, int x, int y)
        {
            return stage.GetCell(x, y)?.Kind == Cells.CellKind.Floor
                && stage.PathManager.CanBeCreated(_oven, x, y);
        }

        public EntityBase Create(Stage stage, int x, int y)
        {
            _oven.CellX = x;
            _oven.CellY = y;
            _oven.X = x;
            _oven.Y = y;
            _oven.Rotation = GeomUtils.GetAngle(stage.ActiveEntity.Cell, _oven.Cell);
            return _oven;
        }
        
        public bool CheckBuildDistance(Point player, Point target)
        {
            return MathUtils.L2(player, target) < 3;
        }
    }
}
