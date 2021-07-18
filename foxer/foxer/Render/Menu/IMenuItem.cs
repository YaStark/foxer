using System.Drawing;

namespace foxer.Render.Menu
{
    public interface IMenuItem
    {
        void Render(INativeCanvas canvas, RectangleF bounds);
        void Update(int delayMs);
        bool Touch(PointF pt, RectangleF bounds);
    }
}
