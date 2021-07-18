using System.Drawing;

namespace foxer.Render.Menu
{
    public interface IMenu : ITouchController
    {
        void Render(INativeCanvas canvas, SizeF size);
        void Update(int delayMs);
    }
}
