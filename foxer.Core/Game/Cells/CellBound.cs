namespace foxer.Core.Game.Cells
{
    public class CellBound : CellBase
    {
        public CellBound(int x, int y)
            : base(CellKind.Bound, x, y, false)
        {
        }
    }
}
