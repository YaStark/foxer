using foxer.Core.Game.Cells;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game.Generator.StageGenerator
{
    public class StageRoadGenerator : StageGeneratorBase
    {
        public override void Generate(Stage stage, StageGeneratorArgs args)
        {
            if (args.Doors.Count > 1)
            {
                var doors = new List<CellDoor>(args.Doors);
                var mainDoors = args.Doors
                    .SelectMany(a => args.Doors.Select(b => new {
                        A = a,
                        B = b,
                        Distance = DoorsDistanceL1(new Point(a.X, a.Y), new Point(b.X, b.Y))
                    }))
                    .Where(x => x.Distance > 0)
                    .OrderByDescending(x => x.Distance)
                    .First();

                doors.Remove(mainDoors.A);
                doors.Remove(mainDoors.B);
                GenerateRoad(
                    stage,
                    new Point(mainDoors.A.X, mainDoors.A.Y),
                    new Point(mainDoors.B.X, mainDoors.B.Y),
                    args.Rnd);

                foreach (var door in doors)
                {
                    var cell = GetNearestRoad(stage, door.X, door.Y);
                    GenerateRoad(stage, cell, new Point(door.X, door.Y), args.Rnd);
                }
            }
            else if (args.Doors.Count > 0)
            {
                if (args.PlayerSpawnLocation.HasValue)
                {
                    int px = args.PlayerSpawnLocation.Value.X;
                    int py = args.PlayerSpawnLocation.Value.Y;
                    GenerateRoad(
                        stage,
                        new Point(args.Doors[0].X, args.Doors[0].Y),
                        new Point(px, py),
                        args.Rnd);

                    stage.Cells[px, py] = new CellRoad(px, py);
                }
                else
                {
                    GenerateRoad(
                        stage,
                        new Point(args.Doors[0].X, args.Doors[0].Y),
                        new Point(stage.Width / 2 + args.Rnd.Next(5) - 2, stage.Height / 2 + args.Rnd.Next(5) - 2),
                        args.Rnd);
                }
            }
        }

        private void GenerateRoad(Stage stage, Point doorA, Point doorB, Random rnd)
        {
            var field = new int[stage.Width, stage.Height];
            var stack = new Stack<Point>();

            field[doorA.X, doorA.Y] = 1;
            stack.Push(doorA);

            while (stack.Any())
            {
                var cell = stack.Pop();
                var value = field[cell.X, cell.Y];

                TryPush(stage, stack, field, cell.X + 1, cell.Y, value + rnd.Next(5));
                TryPush(stage, stack, field, cell.X, cell.Y - 1, value + rnd.Next(5));
                TryPush(stage, stack, field, cell.X, cell.Y + 1, value + rnd.Next(5));
                TryPush(stage, stack, field, cell.X - 1, cell.Y, value + rnd.Next(5));
            }

            Point pt = new Point(doorB.X, doorB.Y);
            while (field[pt.X, pt.Y] != 1)
            {
                pt = GetNextNearestMinimum(stage, field, pt.X, pt.Y);
                if (stage.Cells[pt.X, pt.Y].CanBuildRoad)
                {
                    stage.Cells[pt.X, pt.Y] = new CellRoad(pt.X, pt.Y);
                }
            }
        }

        private Point GetNextNearestMinimum(Stage stage, int[,] field, int i, int j)
        {
            List<Point> items = new List<Point>();
            if (stage.CheckArrayBounds(i + 1, j)) items.Add(new Point(i + 1, j));
            if (stage.CheckArrayBounds(i, j - 1)) items.Add(new Point(i, j - 1));
            if (stage.CheckArrayBounds(i, j + 1)) items.Add(new Point(i, j + 1));
            if (stage.CheckArrayBounds(i - 1, j)) items.Add(new Point(i - 1, j));

            field[i, j] = int.MaxValue;
            return items.Where(x => field[x.X, x.Y] != 0).OrderBy(x => field[x.X, x.Y]).First();
        }

        private Point GetNearestRoad(Stage stage, int x, int y)
        {
            List<Point> cells = new List<Point>();
            for (int i = 0; i < stage.Width; i++)
            {
                for (int j = 0; j < stage.Height; j++)
                {
                    if (stage.Cells[i, j].Kind == CellKind.Road)
                    {
                        cells.Add(new Point(i, j));
                    }
                }
            }

            var pt = new Point(x, y);
            return cells
                .OrderBy(cell => DoorsDistanceL1(cell, pt))
                .FirstOrDefault();
        }

        private void TryPush(Stage stage, Stack<Point> stack, int[,] field, int i, int j, int suffixValue)
        {
            if (!stage.CheckArrayBounds(i, j)
               || field[i, j] > 0)
            {
                return;
            }

            var cell = stage.GetCell(i, j);
            if (cell.CanBuildRoad)
            {
                field[i, j] = cell.BuildRoadPrice + suffixValue;
                stack.Push(new Point(i, j));
            }
            else
            {
                field[i, j] = int.MaxValue;
            }
        }

        private static double DoorsDistanceL1(Point a, Point b)
        {
            return Math.Abs(b.X - a.X) + Math.Abs(b.Y - a.Y);
        }
    }
}
