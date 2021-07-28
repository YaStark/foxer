using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using foxer.Render.Helpers;

namespace foxer.Render
{
    public class GrassRoofRenderer : EntityRendererBase<GrassRoofEntity>
    {
        private static readonly RotatingSpriteRendererHelper _common = new RotatingSpriteRendererHelper(
            Properties.Resources.sprite_grass_roof, 4);

        private static readonly RotatingSpriteRendererHelper _outer = new RotatingSpriteRendererHelper(
            Properties.Resources.sprite_grass_roof_outer, 4);

        private static readonly RotatingSpriteRendererHelper _inner = new RotatingSpriteRendererHelper(
            Properties.Resources.sprite_grass_roof_inner, 4);

        protected override void Render(INativeCanvas canvas, GrassRoofEntity entity, RectangleF bounds)
        {
            bounds = GeomUtils.Deflate(bounds, -bounds.Width / 2, -bounds.Height / 2);
            switch(entity.RoofKind)
            {
                case RoofKind.Common:
                    _common.Render(canvas, bounds, entity.Rotation);
                    return;

                case RoofKind.Inner:
                    _inner.Render(canvas, bounds, entity.Rotation);
                    return;

                case RoofKind.Outer:
                    _outer.Render(canvas, bounds, entity.Rotation);
                    return;
            }
        }
    }
}
