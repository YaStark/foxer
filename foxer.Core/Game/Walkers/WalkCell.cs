using foxer.Core.Game.Entities;
using System.Drawing;

namespace foxer.Core.Game
{
    public class WalkCell : IWalkBuilderCell
    {
        public int X { get; }

        public int Y { get; }

        public Point Cell { get; }

        public IPlatform Platform { get; }

        public WalkCell(Point cell, IPlatform platform)
        {
            X = cell.X;
            Y = cell.Y;
            Cell = cell;
            Platform = platform;
        }

        public WalkCell(int x, int y, IPlatform platform)
        {
            X = x;
            Y = y;
            Cell = new Point(x, y);
            Platform = platform;
        }
    }
}
