using foxer.Core.Game.Animation;
using foxer.Core.Game.Cells;

namespace foxer.Core.Game.Entities
{
    public class StoneSmallEntity : EntityBase
    {
        public int Kind { get; }

        public SimpleAnimation Shake { get; }

        public StoneSmallEntity(CellBase cell, int kind)
            : base(cell.X, cell.Y, 0)
        {
            Kind = kind;
        }
    }
}
