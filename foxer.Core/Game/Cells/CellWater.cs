namespace foxer.Core.Game.Cells
{
    public class CellWater : CellBase
    {
        public int Misc { get; set; }

        public CellWater(int x, int y, int misc)
            : base(CellKind.Water, x, y, false)
        {
            Misc = misc;
        }
    }
}
