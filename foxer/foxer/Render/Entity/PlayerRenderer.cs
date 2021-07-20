using foxer.Core.Game.Entities;
using foxer.Render.Helpers;
using System.Drawing;

namespace foxer.Render
{
    public class PlayerRenderer : EntityRendererBase<PlayerEntity>
    {
        private static readonly RotatingAnimationSpriteRendererHelper _walking = new RotatingAnimationSpriteRendererHelper(
            Properties.Resources.sprite_player_walk, 4, 4);

        private static readonly RotatingAnimationSpriteRendererHelper _idle = new RotatingAnimationSpriteRendererHelper(
            Properties.Resources.sprite_player_idle, 4, 4);
        
        protected override void Render(INativeCanvas canvas, PlayerEntity entity, RectangleF bounds)
        {
            bounds = ScaleBounds(bounds, 1.5f);
            if (entity.ActiveAnimation == entity.Walk)
            {
                _walking.RenderImageByRotation(canvas, bounds, entity.Rotation, entity.Walk);
            }
            else
            {
                _idle.RenderImageByRotation(canvas, bounds, entity.Rotation, entity.Idle);
            }
        }
    }
}
