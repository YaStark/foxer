namespace foxer.Core.Game.Cells
{
    public class CellFloor : CellBase
    {
        public int CellFloorKind { get; }

        public CellFloor(int x, int y, int cellFloorKind)
            : base(CellKind.Floor, x, y, true)
        {
            CellFloorKind = cellFloorKind;
        }
    }
}
