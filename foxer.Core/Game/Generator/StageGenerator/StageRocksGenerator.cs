using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using System.Linq;

namespace foxer.Core.Game.Generator.StageGenerator
{
    public class StageRocksGenerator : StageGeneratorBase
    {
        public override void Generate(Stage stage, StageGeneratorArgs args)
        {
            var floorCells = stage.Cells.OfType<CellBase, CellFloor>();
            int countSmall = (int)(stage.Width * 2 * (0.5 + args.Rnd.NextDouble() / 2));
            int countBig = (int)(stage.Width / 2 * (0.5 + args.Rnd.NextDouble() / 2));
            int k = 0;
            while((countSmall > 0 || countBig > 0) && floorCells.Any())
            {
                int i = args.Rnd.Next(floorCells.Count);
                if (countSmall > 0
                    && k < args.Rnd.Next(-2, 3))
                {
                    if(stage.TryCreateNow(new StoneSmallEntity(floorCells[i], args.Rnd.Next())))
                    {
                        countSmall--;
                        k++;
                    }
                }
                else if(countBig > 0)
                {
                    if (stage.TryCreateNow(new StoneBigEntity(floorCells[i], args.Rnd.Next())))
                    {
                        countBig--;
                        k--;
                    }
                }

                floorCells.RemoveAt(i);
            }
        }
    }
}
