using foxer.Core.Game.Cells;

namespace foxer.Core.Game.Generator.StageGenerator
{
    public class StageFloorGenerator : StageGeneratorBase
    {
        public override void Generate(Stage stage, StageGeneratorArgs args)
        {
            for (int i = 1; i < stage.Width - 1; i++)
            {
                for (int j = 1; j < stage.Height - 1; j++)
                {
                    if (stage.Cells[i, j] == null)
                    {
                        stage.Cells[i, j] = new CellFloor(i, j, args.Rnd.Next());
                    }
                }
            }
        }
    }
}
