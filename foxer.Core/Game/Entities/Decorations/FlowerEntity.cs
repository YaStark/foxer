using foxer.Core.Game.Cells;

namespace foxer.Core.Game.Entities
{
    public class FlowerEntity : EntityBase
    {
        public int Kind { get; }

        public FlowerEntity(CellBase cell, int kind)
            : base(cell.X, cell.Y, 0)
        {
            Kind = kind;
        }

        public override bool UseHitbox()
        {
            return false;
        }
    }
}
