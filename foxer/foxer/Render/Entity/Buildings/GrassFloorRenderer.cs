using System.Drawing;
using foxer.Core.Game.Entities;

namespace foxer.Render
{
    public class GrassFloorRenderer : EntityRendererBase<GrassFloorEntity>
    {
        private static readonly byte[] _image = Properties.Resources.grass_floor;

        protected override void Render(INativeCanvas canvas, GrassFloorEntity entity, RectangleF bounds)
        {
            canvas.DrawImage(_image, ScaleBounds(bounds, 1.3f));
        }
    }
}
