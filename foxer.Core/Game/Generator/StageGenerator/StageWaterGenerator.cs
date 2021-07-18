using foxer.Core.Game.Cells;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Core.Game.Generator.StageGenerator
{
    public class StageWaterGenerator : StageGeneratorBase, IFloodGeneratorUser
    {
        private readonly FloodGenerator _floodGenerator;

        public StageWaterGenerator(int stageSize)
        {
            _floodGenerator = new FloodGenerator(stageSize, this);
        }

        public override void Generate(Stage stage, StageGeneratorArgs args)
        {
            _floodGenerator.Generate(stage, args.Rnd);
        }

        public bool CanPropagateTo(Stage stage, Random rnd, int x, int y)
        {
            return CanPropagateTo(stage, x + 1, y)
                && CanPropagateTo(stage, x - 1, y)
                && CanPropagateTo(stage, x, y + 1)
                && CanPropagateTo(stage, x, y - 1);
        }

        public CellBase CreateCell(CreateCellEventArgs args)
        {
            return new CellWater(args.X, args.Y, args.NeighboursCount < 4 ? args.Rnd.Next() : 0);
        }

        public IEnumerable<Point> GetNextEpicenter(Stage stage, Random rnd, RefValue<int> count)
        {
            while(count.Value == 0)
            {
                yield return new Point(rnd.Next(1, stage.Width - 2), rnd.Next(1, stage.Height - 2));
            }
        }

        private bool CanPropagateTo(Stage stage, int x, int y)
        {
            var cell = stage.GetCell(x, y);
            return cell != null && cell.Kind != CellKind.Road;
        }
    }
}
