using foxer.Core.Game.Cells;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Core.Game.Generator
{
    public interface IFloodGeneratorUser
    {
        CellBase CreateCell(CreateCellEventArgs args);
        IEnumerable<Point> GetNextEpicenter(Stage stage, Random rnd, RefValue<int> count);
        bool CanPropagateTo(Stage stage, Random rnd, int x, int y);
    }
}
