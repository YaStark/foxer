using foxer.Core.Game.Entities;
using System.Drawing;

namespace foxer.Core.Game
{
    public class WalkBuilderCell : IWalkBuilderCell
    {
        public WalkBuilderCell[] LTRB { get; } = new WalkBuilderCell[4];

        public WalkBuilderCell Left { get => LTRB[0]; set => LTRB[0] = value; }

        public WalkBuilderCell Top { get => LTRB[1]; set => LTRB[1] = value; }

        public WalkBuilderCell Right { get => LTRB[2]; set => LTRB[2] = value; }

        public WalkBuilderCell Bottom { get => LTRB[3]; set => LTRB[3] = value; }

        public int X => Cell.X;

        public int Y => Cell.Y;

        public Point Cell { get; }

        public int Weight { get; set; }

        public int Distance { get; set; } = -1;

        public IPlatform Platform { get; set; }

        public WalkBuilderCell(Point pt, int weight)
        {
            Cell = pt;
            Weight = weight;
        }

        public bool IsEmpty()
        {
            return Platform == null;
        }

        public void Clear()
        {
            Weight = 0;
            Distance = -1;
            Platform = null;
            for (int i = 0; i < LTRB.Length; i++)
            {
                LTRB[i] = null;
            }
        }
    }
}
