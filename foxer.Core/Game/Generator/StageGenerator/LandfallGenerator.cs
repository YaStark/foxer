using System;
using System.Collections.Generic;
using System.Drawing;
using foxer.Core.Game.Cells;
using foxer.Core.Utils;

namespace foxer.Core.Game.Generator.StageGenerator
{
    public class LandfallGenerator : StageGeneratorBase, IFloodGeneratorUser
    {
        private readonly FloodGenerator _floodGenerator;

        public LandfallGenerator()
        {
            _floodGenerator = new FloodGenerator(3, this);
        }

        public bool CanPropagateTo(Stage stage, Random rnd, int x, int y)
        {
            var cell = stage.GetCell(x, y);
            if (cell == null) return false;
            switch(cell.Kind)
            {
                case CellKind.Bound:
                case CellKind.Floor:
                    return true;

                default:
                    return false;
            }
        }

        public CellBase CreateCell(CreateCellEventArgs args)
        {
            return null;
        }

        public override void Generate(Stage stage, StageGeneratorArgs args)
        {
            _floodGenerator.Generate(stage, args.Rnd);
        }

        public IEnumerable<Point> GetNextEpicenter(Stage stage, Random rnd, RefValue<int> count)
        {
            int iterations = rnd.Next(stage.Width / 5);
            foreach (var pt in FloodGenerator.GetEpicenters(
                iterations,
                count,
                () => GetRndLandfallSource(stage, rnd)))
            {
                yield return pt;
            }
        }

        private Point GetRndLandfallSource(Stage stage, Random rnd)
        {
            var value = rnd.NextDouble();
            if (value < 0.25)
            {
                return new Point(0, rnd.Next(stage.Height));
            }

            if (value < 0.5)
            {
                return new Point(stage.Width - 1, rnd.Next(stage.Height));
            }

            if (value < 0.75)
            {
                return new Point(rnd.Next(stage.Width), 0);
            }

            return new Point(rnd.Next(stage.Width), stage.Height - 1);
        }
    }
}
