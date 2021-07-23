using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Render.Helpers;

namespace foxer.Render
{
    public class GrassWallRenderer : EntityRendererBase<GrassWallEntity>
    {
        private static readonly RotatingSpriteRendererHelper _rotate = new RotatingSpriteRendererHelper(
            Properties.Resources.sprite_wall, 4);

        protected override void Render(INativeCanvas canvas, GrassWallEntity entity, RectangleF bounds)
        {
            _rotate.Render(canvas, ScaleBounds(bounds, 2f), entity.Rotation);
        }
    }
}
