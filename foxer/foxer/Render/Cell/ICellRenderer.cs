using foxer.Core.Game.Cells;
using System.Drawing;

namespace foxer.Render
{
    public interface ICellRenderer
    {
        bool CanRender<T>(T item);
        void Render(CellBase cell, INativeCanvas canvas, RectangleF bounds, CellBase left, CellBase top, CellBase right, CellBase bottom);
    }
}
