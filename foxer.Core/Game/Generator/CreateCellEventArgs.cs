using foxer.Core.Game.Cells;
using System;

namespace foxer.Core.Game.Generator
{
    public class CreateCellEventArgs
    {
        public CellBase SourceCell { get; }
        public int X { get; }
        public int Y { get; }
        public Random Rnd { get; }
        public int NeighboursCount { get; }

        public CreateCellEventArgs(CellBase source, int x, int y, Random rnd, int neighboursCount)
        {
            SourceCell = source;
            X = x;
            Y = y;
            Rnd = rnd;
            NeighboursCount = neighboursCount;
        }
    }
}
