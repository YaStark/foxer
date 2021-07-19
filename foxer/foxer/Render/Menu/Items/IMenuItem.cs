using foxer.Core.Game;
using System.Drawing;

namespace foxer.Render.Menu
{
    public interface IMenuItem
    {
        void Render(INativeCanvas canvas, MenuItemInfoArgs args);
        void Update(int delayMs);
        bool Touch(PointF pt, MenuItemInfoArgs args);
    }

    public class MenuItemInfoArgs
    {
        public Stage Stage { get; }
        public RectangleF Bounds { get; }
        public SizeF CellSize { get; }

        public MenuItemInfoArgs(Stage stage, RectangleF bounds, SizeF cellSize)
        {
            Stage = stage;
            Bounds = bounds;
            CellSize = cellSize;
        }
    }
}
