using foxer.Core.Game.Entities;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game
{
    public class WalkBuilderWithMultipleTargets : WalkBuilderBase
    {
        private readonly List<IWalkBuilderCell> _incompletedTargets = new List<IWalkBuilderCell>();

        public IWalkBuilderCell[] Targets { get; }

        public WalkBuilderWithMultipleTargets(
            Stage stage,
            EntityBase host,
            ICellWeightProvider weightProvider,
            ICellAccessibleProvider accessibleProvider,
            IWalkBuilderCell[] targets)
            : base(stage, host, weightProvider, accessibleProvider)
        {
            Targets = targets;
            _incompletedTargets.AddRange(targets);
            BuildField();
        }

        public Point[] GetShortestPath(out int index)
        {
            Point[] shortestPath = null;
            index = -1;
            for (int i = 0; i < Targets.Length; i++)
            {
                var path = BuildPath(Field[Targets[i].X, Targets[i].Y].FirstOrDefault(c => c.Platform == Targets[i].Platform));
                if(path != null)
                {
                    if(shortestPath == null
                        || shortestPath.Length > path.Length)
                    {
                        shortestPath = path;
                        index = i;
                    }
                }
            }

            return shortestPath;
        }

        public Point[] GetShortestPath()
        {
            return GetShortestPath(out _);
        }

        protected override bool CheckDestination(WalkBuilderCell cell)
        {
            var targetCell = _incompletedTargets.FirstOrDefault(t => t.Cell == cell.Cell && t.Platform == cell.Platform);
            if (targetCell != null) _incompletedTargets.Remove(targetCell);
            return !_incompletedTargets.Any();
        }

        protected override bool CanUseCell(WalkBuilderCell cell)
        {
            return true;
        }
    }
}
