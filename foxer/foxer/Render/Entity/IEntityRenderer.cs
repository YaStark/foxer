using foxer.Core.Game.Entities;
using System.Drawing;

namespace foxer.Render
{
    public interface IEntityRenderer
    {
        bool CanRender<T>(T item);
        void Render(INativeCanvas canvas, EntityBase entity, RectangleF bounds);
    }
}
