using System.Drawing;
using foxer.Core.Game.Entities;

namespace foxer.Render
{
    public class StoneOvenRenderer : EntityRendererBase<StoneOvenEntity>
    {
        private static readonly byte[] _image = Properties.Resources.stone_oven;

        protected override void Render(INativeCanvas canvas, StoneOvenEntity entity, RectangleF bounds)
        {
            canvas.DrawImage(_image, ScaleBounds(bounds, 1.3f));
        }
    }
}
