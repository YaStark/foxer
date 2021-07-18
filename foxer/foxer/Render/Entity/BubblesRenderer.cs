using System;
using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Render.Helpers;

namespace foxer.Render
{
    public class BubblesRenderer : EntityRendererBase<BubblesEntity>
    {
        private static readonly AnimationRendererHelper _rendererHelper;

        static BubblesRenderer()
        {
            _rendererHelper = new AnimationRendererHelper(
                Properties.Resources.bubbles_0,
                Properties.Resources.bubbles_1,
                Properties.Resources.bubbles_2,
                Properties.Resources.bubbles_3,
                Properties.Resources.bubbles_4,
                Properties.Resources.bubbles_5,
                Properties.Resources.bubbles_6,
                Properties.Resources.bubbles_7,
                Properties.Resources.bubbles_8);
        }

        protected override void Render(INativeCanvas canvas, BubblesEntity entity, RectangleF bounds)
        {
            if(entity.ActiveAnimation == entity.Bubble)
            {
                var image = _rendererHelper.GetImage(entity.Bubble);
                canvas.DrawImage(image, bounds);
            }
        }
    }
}
