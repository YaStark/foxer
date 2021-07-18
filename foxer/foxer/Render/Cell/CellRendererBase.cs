using foxer.Core.Game.Cells;
using foxer.Pages;
using System.Drawing;

namespace foxer.Render
{
    public abstract class CellRendererBase<TCell> : ICellRenderer
        where TCell : CellBase
    {
        public virtual bool CanRender<T>(T item)
        {
            return item.GetType() == typeof(TCell);
        }

        public void Render(CellBase cell, INativeCanvas canvas, RectangleF bounds, CellBase left, CellBase top, CellBase right, CellBase bottom)
        {
            RenderCell(cell as TCell, canvas, bounds, left, top, right, bottom);
            if(PageGame.EnableDebugGraphics)
            {
                if (cell == null) return;
                bounds.Inflate(-0.2f, -0.2f);
                canvas.DrawRectangle(bounds, Color.Black);
                bounds.Inflate(-0.1f, -0.1f);
                canvas.DrawRectangle(bounds, cell.CanWalk ? Color.Green : Color.Red);
            }
        }

        protected abstract void RenderCell(
            TCell cell,
            INativeCanvas canvas,
            RectangleF bounds,
            CellBase left,
            CellBase top,
            CellBase right,
            CellBase bottom);
    }
}
