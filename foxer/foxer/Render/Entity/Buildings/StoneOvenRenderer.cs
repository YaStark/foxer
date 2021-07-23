using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Render.Helpers;

namespace foxer.Render
{
    public class StoneOvenRenderer : EntityRendererBase<StoneOvenEntity>
    {
        private static readonly RotatingSpriteRendererHelper _rotate = new RotatingSpriteRendererHelper(
            Properties.Resources.sprite_stone_oven, 4);

        protected override void Render(INativeCanvas canvas, StoneOvenEntity entity, RectangleF bounds)
        {
            _rotate.Render(canvas, bounds, entity.Rotation);
        }
    }
}
