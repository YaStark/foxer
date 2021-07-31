using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game
{
    public abstract class WalkBuilderBase : IDisposable
    {
        private readonly List<WalkBuilderCell> _cells = new List<WalkBuilderCell>();

        protected WalkBuilderCell[,][] Field { get; }
        protected WalkBuilderCell InitialCell { get; }

        public Stage Stage { get; }
        public EntityBase Host { get; }
        public ICellWeightProvider WeightProvider { get; }
        public ICellAccessibleProvider AccessibleProvider { get; }

        protected WalkBuilderBase(
            Stage stage,
            EntityBase host,
            ICellWeightProvider weightProvider,
            ICellAccessibleProvider accessibleProvider)
        {
            Stage = stage;
            Host = host;
            WeightProvider = weightProvider;
            AccessibleProvider = accessibleProvider;
            Field = stage.WalkBuilderFieldFactory.Get(host.GetType());

            InitialCell = Field[Host.CellX, Host.CellY][0];
            InitialCell.Weight = 1;
            InitialCell.Platform = Stage.GetPlatform(Host);

            _cells.Add(InitialCell);
        }

        protected void BuildField()
        {
            var queue = new Queue<WalkBuilderCell>();
            queue.Enqueue(InitialCell);
            
            while (queue.Any())
            {
                var cell = queue.Dequeue();
                if(CheckDestination(cell))
                {
                    break;
                }

                TryFillArray(cell, 0, cell.X - 1, cell.Y, queue, cell.Weight);
                TryFillArray(cell, 1, cell.X, cell.Y - 1, queue, cell.Weight);
                TryFillArray(cell, 2, cell.X + 1, cell.Y, queue, cell.Weight);
                TryFillArray(cell, 3, cell.X, cell.Y + 1, queue, cell.Weight);
            }
        }

        protected abstract bool CheckDestination(WalkBuilderCell cell);

        protected abstract bool CanUseCell(WalkBuilderCell cell);

        protected IEnumerable<IWalkBuilderCell> GetUsedPoints()
        {
            return _cells;
        }

        protected IEnumerable<WalkBuilderCell> GetLightestPointsAtDistance(int minDistance, int maxDistance, int maxCount)
        {
            var cells = _cells
                .Where(cell => MathUtils.L1(cell.Cell, InitialCell.Cell) >= minDistance)
                .Where(cell => MathUtils.L1(cell.Cell, InitialCell.Cell) <= maxDistance);

            if (cells.Count() < maxCount)
            {
                return cells;
            }

            return cells.OrderBy(cell => cell.Weight).Take(maxCount);
        }

        protected Point[] BuildPath(WalkBuilderCell finalCell)
        {
            if (finalCell == null)
            {
                return null;
            }

            var cell = Field[finalCell.X, finalCell.Y].FirstOrDefault(c => c.Platform == finalCell.Platform);
            if (cell == null)
            {
                return null;
            }

            List<WalkBuilderCell> steps = new List<WalkBuilderCell>();
            steps.Add(finalCell);
            while (cell != InitialCell)
            {
                cell = GetNextStep(cell, steps);
                if (cell == null)
                {
                    return null;
                }

                steps.Add(cell);
            }

            steps.Remove(InitialCell);
            steps.Reverse();
            return steps.Select(x => x.Cell).ToArray();
        }

        private WalkBuilderCell GetNextStep(WalkBuilderCell origin, List<WalkBuilderCell> steps)
        {
            var nearestCells = origin.LTRB.Where(c => c != null && !steps.Contains(c));
            if (!nearestCells.Any())
            {
                return null;
            }

            return nearestCells.OrderBy(c => c.Weight).First();
        }

        private void TryFillArray(WalkBuilderCell origin, int i, int x, int y, Queue<WalkBuilderCell> queue, int suffixValue)
        {
            if (!Stage.CheckArrayBounds(x, y))
            {
                return;
            }

            var point = new Point(x, y);
            var platform = Stage.GetPlatformOnTransit(Host, origin.Cell, point, origin.Platform);
            if (platform == null)
            {
                return;
            }

            var value = suffixValue + GetCellWeight(x, y, platform);
            if (origin.LTRB[i] != null && origin.LTRB[i].Weight <= value)
            {
                return;
            }

            if (AccessibleProvider?.CanWalk(Stage, Host, x, y, platform) == false)
            {
                return;
            }

            WalkBuilderCell cell = Field[x, y].FirstOrDefault(c => c.Platform == platform);
            if (cell != null)
            {
                if (cell.Weight <= value)
                {
                    origin.LTRB[i] = cell;
                    cell.LTRB[(i + 2) % 4] = origin;
                    return;
                }
                else
                {
                    cell.Weight = value;
                }
            }

            if (cell == null)
            {
                cell = Field[x, y][Field[x, y].Count(c => !c.IsEmpty())];
                cell.Weight = value;
                cell.Platform = platform;
                _cells.Add(cell);
            }

            if (CanUseCell(cell))
            {
                origin.LTRB[i] = cell;
                queue.Enqueue(origin.LTRB[i]);
                cell.LTRB[(i + 2) % 4] = origin;
            }
        }

        private int GetCellWeight(int x, int y, IPlatform platform)
        {
            if (WeightProvider != null)
            {
                return WeightProvider.GetCellWeight(Stage, Host, x, y, platform);
            }

            return (int)(Math.Atan(Stage.StressManager.GetStressLevelInCell(Host, x, y)) / Math.PI * 5) + 6;
        }

        public void Dispose()
        {
            foreach(var cell in _cells)
            {
                cell.Clear();
            }
        }
    }
}
