using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game
{
    public class RandomWalkBuilder : WalkBuilderBase
    {
        public int MaxDistance { get; }

        public RandomWalkBuilder(
            Stage stage, 
            ICellWeightProvider weightProvider, 
            ICellAccessibleProvider accesibleProvider, 
            EntityBase host, 
            int maxDistance)
            : base(stage, host, weightProvider, accesibleProvider)
        {
            MaxDistance = maxDistance;
            BuildField();
        }

        public IEnumerable<IWalkBuilderCell> GetPoints()
        {
            return GetUsedPoints();
        }

        public Point[] BuildWalkPath(IWalkBuilderCell cell)
        {
            return BuildPath(Field[cell.X, cell.Y].FirstOrDefault(c => c.Platform == cell.Platform));
        }
        
        protected override bool CheckDestination(WalkBuilderCell cell)
        {
            return false;
        }

        protected override bool CanUseCell(WalkBuilderCell cell)
        {
            return MathUtils.L1(cell.Cell, Host.Cell) < MaxDistance;
        }

        public IEnumerable<IWalkBuilderCell> GetLightestPoints(int min, int max, int count)
        {
            return GetLightestPointsAtDistance(min, max, count);
        }
    }
}
