using foxer.Core.Game.Cells;

namespace foxer.Core.Game.Generator.StageGenerator
{
    public class StageBoundsGenerator : StageGeneratorBase
    {
        public override void Generate(Stage stage, StageGeneratorArgs args)
        {
            for (int i = 0; i < stage.Width; i++)
            {
                stage.Cells[i, 0] = new CellBound(i, 0);
                stage.Cells[i, stage.Height - 1] = new CellBound(i, stage.Height - 1);
            }

            for (int j = 0; j < stage.Height; j++)
            {
                stage.Cells[0, j] = new CellBound(0, j);
                stage.Cells[stage.Width - 1, j] = new CellBound(stage.Width - 1, j);
            }

            foreach (CellDoor door in args.Doors)
            {
                stage.Cells[door.X, door.Y] = door;
            }
        }
    }
}
