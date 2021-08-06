using foxer.Core.Game.Entities;
using foxer.Properties;
using foxer.Render.Helpers;
using System.Drawing;

namespace foxer.Render
{
    public class CowRenderer : EntityRendererBase<CowEntity>
    {
        private static readonly EntityRendererHelper<CowEntity> _renderer = new EntityRendererHelper<CowEntity>()
            .AddRotatingAnimation(e => e.ActiveAnimation == e.Walk, Resources.sprite_cow_walk, 4, 4)
            .AddRotatingAnimation(e => e.ActiveAnimation == e.Idle, Resources.sprite_cow_idle, 4, 4)
            .UseAsDefault()
            .AddRotatingAnimation(e => e.ActiveAnimation == e.HeadUp, Resources.sprite_cow_headup, 3, 4)
            .AddRotatingAnimation(e => e.ActiveAnimation == e.HeadDown, Resources.sprite_cow_headup, 3, 4, true);

        protected override void Render(INativeCanvas canvas, CowEntity entity, RectangleF bounds)
        {
            bounds = ScaleBounds(bounds, 1.8f);
            _renderer.Render(canvas, bounds, entity);
        }
    }
}
