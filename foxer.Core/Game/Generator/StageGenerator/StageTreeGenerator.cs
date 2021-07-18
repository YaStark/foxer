using foxer.Core.Game.Cells;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Core.Game.Generator.StageGenerator
{
    public class StageTreeGenerator : StageGeneratorBase, IFloodGeneratorUser
    {
        private readonly FloodGenerator _floodGenerator;

        private const float RND_TREE_PROBABILITY = 0.5f;

        public StageTreeGenerator()
        {
            _floodGenerator = new FloodGenerator(5, this);
        }

        public override void Generate(Stage stage, StageGeneratorArgs args)
        {
            _floodGenerator.Generate(stage, args.Rnd);
        }

        public bool CanPropagateTo(Stage stage, Random rnd, int x, int y)
        {
            var cell = stage.GetCell(x, y);
            return cell != null
                && cell.Kind == CellKind.Floor;
        }

        public CellBase CreateCell(CreateCellEventArgs args)
        {
            return new CellTree(args.SourceCell);
        }

        public IEnumerable<Point> GetNextEpicenter(Stage stage, Random rnd, RefValue<int> count)
        {
            int iterations = rnd.Next(2, stage.Width / 2);
            foreach(var pt in FloodGenerator.GetEpicenters(
                iterations, 
                count, 
                () => new Point(rnd.Next(stage.Width), rnd.Next(stage.Height))))
            {
                yield return pt;
            }
        }
    }
}
