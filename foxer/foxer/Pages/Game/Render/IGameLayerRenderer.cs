using foxer.Render;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Pages
{
    public interface IGameLayerRenderer
    {
        void Render(INativeCanvas canvas, IEnumerable<Point> cells);
        bool Touch(float x, float y, Rectangle viewportBounds);
        bool Enabled { get; }
    }
}