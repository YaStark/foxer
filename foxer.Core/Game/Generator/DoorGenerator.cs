using foxer.Core.Game.Cells;

namespace foxer.Core.Game.Generator
{
    public class DoorGenerator
    {
        public (CellDoor, CellDoor) LinkStagesLeftRight(Stage stageLeft, Stage stageRight, int y)
        {
            var doorLeft = new CellDoor(stageLeft.Width - 1, y, DoorKind.LeftRight)
            {
                LinkedStage = stageRight
            };

            var doorRight = new CellDoor(0, y, DoorKind.RightLeft)
            {
                LinkedStage = stageLeft,
                LinkedDoor = doorLeft,
            };

            doorLeft.LinkedDoor = doorRight;
            return (doorLeft, doorRight);
        }

        public (CellDoor, CellDoor) LinkStagesTopBottom(Stage stageTop, Stage stageBottom, int x)
        {
            var doorTop = new CellDoor(x, 0, DoorKind.TopBottom)
            {
                LinkedStage = stageBottom
            };

            var doorBottom = new CellDoor(x, stageBottom.Width - 1, DoorKind.BottomTop)
            {
                LinkedStage = stageTop,
                LinkedDoor = doorTop,
            };

            doorTop.LinkedDoor = doorBottom;
            return (doorTop, doorBottom);
        }
    }
}
