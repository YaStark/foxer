using foxer.Core.Game.Entities;
using System.Drawing;

namespace foxer.Core.Game
{
    public class WalkCell : IWalkBuilderCell
    {
        public int X => Cell.X;

        public int Y => Cell.Y;

        public Point Cell { get; }

        public IPlatform Platform { get; }

        public WalkCell(Point cell, IPlatform platform)
        {
            Cell = cell;
            Platform = platform;
        }

        public WalkCell(int x, int y, IPlatform platform)
        {
            Cell = new Point(x, y);
            Platform = platform;
        }
    }
}
