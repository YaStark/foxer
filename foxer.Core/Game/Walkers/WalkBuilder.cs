using foxer.Core.Game.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game
{
    public class WalkBuilder
    {
        public Stage Stage { get; }
        public EntityBase Host { get; }
        public Point Target { get; }
        public ICellWeightProvider WeightProvider { get; }
        public ICellAccessibleProvider AccessibleProvider { get; }
        public Point[] Path { get; }

        public WalkBuilder(
            Stage stage, 
            EntityBase host, 
            ICellWeightProvider weightProvider,
            ICellAccessibleProvider accessibleProvider, 
            Point target)
        {
            Stage = stage;
            Host = host;
            Target = target;
            WeightProvider = weightProvider;
            AccessibleProvider = accessibleProvider;
            Path = BuildWalkPath(BuildField());
        }

        private int[,] BuildField()
        {
            var field = new int[Stage.Width, Stage.Height];
            var queue = new Queue<Point>();
            queue.Enqueue(Host.Cell);
            field[Host.CellX, Host.CellY] = 1;
            while (queue.Any())
            {
                var pt = queue.Dequeue();
                if (Target == pt)
                {
                    break;
                }

                var value = field[pt.X, pt.Y];
                TryFillArray(pt.X + 1, pt.Y, field, queue, value);
                TryFillArray(pt.X - 1, pt.Y, field, queue, value);
                TryFillArray(pt.X, pt.Y - 1, field, queue, value);
                TryFillArray(pt.X, pt.Y + 1, field, queue, value);
            }

            return field;
        }

        private Point[] BuildWalkPath(int[,] field)
        {
            List<Point> steps = new List<Point>();
            Point? pt0 = Target;
            steps.Add(pt0.Value);
            while (field[pt0.Value.X, pt0.Value.Y] != 1)
            {
                pt0 = GetNextStep(pt0.Value.X, pt0.Value.Y, field);
                if (pt0 == null)
                {
                    return null;
                }

                steps.Add(pt0.Value);
            }
            steps.Remove(Host.Cell);
            steps.Reverse();
            return steps.ToArray();
        }

        private int GetCellWeight(int x, int y)
        {
            if (WeightProvider != null)
            {
                return WeightProvider.GetCellWeight(Stage, Host, x, y);
            }

            return (int)(Math.Atan(Stage.StressManager.GetStressLevelInCell(Host, x, y)) / Math.PI * 5) + 6;
        }

        private bool CanWalkOnCell(int x, int y)
        {
            return AccessibleProvider != null
                ? AccessibleProvider.CanWalk(Stage, Host, x, y)
                : Stage.CheckCanWalkOnCell(Host, x, y);
        }

        private void TryFillArray(int x, int y, int[,] field, Queue<Point> queue, int suffixValue)
        {
            if (!Stage.CheckArrayBounds(x, y)) return;
            var value = suffixValue + GetCellWeight(x, y);

            if ((field[x, y] == 0 || field[x, y] > value)
                && CanWalkOnCell(x, y))
            {
                var point = new Point(x, y);
                field[x, y] = value;
                queue.Enqueue(point);
            }
        }

        private Point? GetNextStep(int x, int y, int[,] field)
        {
            var pts = new[] { new Point(x + 1, y), new Point(x - 1, y), new Point(x, y + 1), new Point(x, y - 1) }
                .Where(pt => Stage.CheckArrayBounds(pt.X, pt.Y) && field[pt.X, pt.Y] != 0);
            return pts.Any() 
                ? pts.OrderBy(pt => field[pt.X, pt.Y]).First()
                : (Point?)null;
        }
    }
}
