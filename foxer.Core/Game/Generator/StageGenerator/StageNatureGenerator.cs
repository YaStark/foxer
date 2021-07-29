using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using System;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game.Generator.StageGenerator
{
    public class StageNatureGenerator : StageGeneratorBase
    {
        private const double PROB_FLOWER_ON_FLOOR = 0.2;
        private const double PROB_SQUIRREL_ON_FLOOR = 0.02;
        private const double PROB_BEE_ON_FLOWER = 0.2;
        private const double PROB_BUBBLES_ON_WATER = 0.2;

        public override void Generate(Stage stage, StageGeneratorArgs args)
        {
            for (int i = 0; i < stage.Width; i++)
            {
                for (int j = 0; j < stage.Height; j++)
                {
                    var cell = stage.GetCell(i, j);

                    TryGenerateGrass(stage, args.Rnd, i, j);

                    //flowers
                    if (args.Rnd.NextDouble() < PROB_FLOWER_ON_FLOOR
                        && cell?.Kind == CellKind.Floor)
                    {
                        stage.TryCreateNow(new FlowerEntity(i, j, args.Rnd.Next()));
                        if (args.Rnd.NextDouble() < PROB_BEE_ON_FLOWER)
                        {
                            stage.TryCreateNow(new BeeEntity(cell.X, cell.Y));
                        }
                    }

                    // bubbles
                    if (args.Rnd.NextDouble() < PROB_BUBBLES_ON_WATER
                        && cell?.Kind == CellKind.Water)
                    {
                        stage.TryCreateNow(new BubblesEntity(cell));
                    }

                    // trees
                    if(cell is CellTree tree)
                    {
                        stage.Cells[i, j] = tree.SourceCell;
                        int age = args.Rnd.Next(TreeEntity.AGE_LARGE * 2);
                        stage.TryCreateNow(new TreeEntity(tree.SourceCell, args.Rnd.Next(), (uint)age));
                    }
                }
            }

            var firstCell = args.PlayerSpawnLocation.HasValue
                ? args.PlayerSpawnLocation.Value
                : new Point(args.Doors[0].X, args.Doors[0].Y);

            GenerateWolfes(stage, args.Rnd, firstCell, 10);
            GenerateSquirrels(stage, args.Rnd, firstCell, 15);
        }

        private void TryGenerateGrass(Stage stage, Random rnd, int i, int j)
        {
            var cell = stage.GetCell(i, j);
            if (!(cell is CellGrass grass))
            {
                return;
            }

            stage.Cells[i, j] = grass.SourceCell;
            stage.TryCreateNow(new GrassEntity(cell, rnd.Next()));
        }

        private void GenerateWolfes(Stage stage, Random rnd, Point firstCell, int count)
        {
            var host = new WolfEntity(firstCell.X, firstCell.Y);
            using (var walker = new RandomWalkBuilder(stage, null, null, host, stage.Width * 2))
            {
                var cells = walker.GetPoints().OrderBy(x => rnd.NextDouble()).Take(count).ToArray();
                foreach (var cell in cells)
                {
                    stage.TryCreateNow(new WolfEntity(cell.X, cell.Y));
                }
            }
        }

        private void GenerateSquirrels(Stage stage, Random rnd, Point firstCell, int count)
        {
            var host = new SquirrelEntity(firstCell.X, firstCell.Y);
            using (var walker = new RandomWalkBuilder(stage, null, null, host, stage.Width * 2))
            {
                var cells = walker.GetPoints().OrderBy(x => rnd.NextDouble()).Take(count).ToArray();
                foreach (var cell in cells)
                {
                    stage.TryCreateNow(new SquirrelEntity(cell.X, cell.Y));
                }
            }
        }
    }
}
