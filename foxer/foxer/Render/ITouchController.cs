using System.Drawing;

namespace foxer.Render
{
    public interface ITouchController
    {
        bool Touch(PointF pt, SizeF size);
    }
}
