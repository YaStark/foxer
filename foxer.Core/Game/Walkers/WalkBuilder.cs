using foxer.Core.Game.Entities;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game
{
    public class WalkBuilder : WalkBuilderBase
    {
        public IWalkBuilderCell Target { get; }

        public WalkBuilder(
            Stage stage, 
            EntityBase host, 
            ICellWeightProvider weightProvider,
            ICellAccessibleProvider accessibleProvider, 
            IWalkBuilderCell targetCell)
            : base(stage, host, weightProvider, accessibleProvider)
        {
            Target = targetCell;
            BuildField();
        }

        public Point[] GetPath()
        {
            return BuildPath(Field[Target.X, Target.Y].FirstOrDefault(c => c.Platform == Target.Platform));
        }

        protected override bool CheckDestination(WalkBuilderCell cell)
        {
            return Target.Cell == cell.Cell && Target.Platform == cell.Platform;
        }

        protected override bool CanUseCell(WalkBuilderCell cell)
        {
            return true;
        }
    }
}
