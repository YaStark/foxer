using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using foxer.Render.Helpers;

namespace foxer.Render
{
    public class GrassRoofRenderer : EntityRendererBase<GrassRoofEntity>
    {
        private static readonly RotatingSpriteRendererHelper _rotate = new RotatingSpriteRendererHelper(
            Properties.Resources.sprite_grass_roof, 4);

        protected override void Render(INativeCanvas canvas, GrassRoofEntity entity, RectangleF bounds)
        {
            bounds = GeomUtils.Deflate(bounds, -bounds.Width / 2, -bounds.Height / 2);
            _rotate.Render(canvas, bounds, entity.Rotation);
        }
    }
}
