using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game
{
    public class RandomWalkBuilder
    {
        private readonly int[,] _field;

        public Stage Stage { get; }
        public EntityBase Host { get; }
        public int MaxDistance { get; }
        public ICellWeightProvider WeightProvider { get; }
        public ICellAccessibleProvider AccessibleProvider { get; }
        public List<Point> Points { get; }

        public RandomWalkBuilder(
            Stage stage, 
            ICellWeightProvider weightProvider, 
            ICellAccessibleProvider accesibleProvider, 
            EntityBase host, 
            int maxDistance)
        {
            Stage = stage;
            Host = host;
            MaxDistance = maxDistance;
            WeightProvider = weightProvider;
            AccessibleProvider = accesibleProvider;
            Points = new List<Point>();
            _field = BuildField();
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
                var value = field[pt.X, pt.Y];
                TryFillArray(pt.X + 1, pt.Y, field, queue, value);
                TryFillArray(pt.X - 1, pt.Y, field, queue, value);
                TryFillArray(pt.X, pt.Y - 1, field, queue, value);
                TryFillArray(pt.X, pt.Y + 1, field, queue, value);
            }

            return field;
        }
        
        public Point[] BuildWalkPath(Point target)
        {
            List<Point> steps = new List<Point>();
            Point? pt0 = target;
            steps.Add(pt0.Value);
            while (_field[pt0.Value.X, pt0.Value.Y] != 1)
            {
                pt0 = GetNextStep(pt0.Value.X, pt0.Value.Y, _field);
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

        private int GetCellWeight(EntityBase walker, int x, int y)
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
            var value = suffixValue + GetCellWeight(Host, x, y);

            if (MathUtils.L1(Host.Cell, new Point(x, y)) < MaxDistance
                && (field[x, y] == 0 || field[x, y] > value)
                && CanWalkOnCell(x, y))
            {
                var point = new Point(x, y);
                field[x, y] = value;
                queue.Enqueue(point);
                Points.Add(point);
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
