using foxer.Core.Game.Entities;
using System.Drawing;

namespace foxer.Render.Helpers
{
    public interface IRendererHelper
    {
        void Render(INativeCanvas canvas, RectangleF bounds, EntityBase entity);
    }
}
