using foxer.Core.Game.Entities;
using foxer.Core.Utils;
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
            return Field.Cast<WalkBuilderCell[]>()
                .SelectMany(a => a)
                .Where(c => !c.IsEmpty());
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
    }
}
