using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Render.Helpers;

namespace foxer.Render
{
    public class BeeRenderer : EntityRendererBase<BeeEntity>
    {
        private static readonly RotatingRendererHelper _rendererHelper = new RotatingRendererHelper();

        static BeeRenderer()
        {
            _rendererHelper.AddImageForRotation(0, Properties.Resources.bee0);
            _rendererHelper.AddImageForRotation(45, Properties.Resources.bee45);
            _rendererHelper.AddImageForRotation(90, Properties.Resources.bee90);
            _rendererHelper.AddImageForRotation(135, Properties.Resources.bee135);
            _rendererHelper.AddImageForRotation(180, Properties.Resources.bee180);
            _rendererHelper.AddImageForRotation(225, Properties.Resources.bee225);
            _rendererHelper.AddImageForRotation(270, Properties.Resources.bee270);
            _rendererHelper.AddImageForRotation(315, Properties.Resources.bee315);
        }

        protected override void Render(INativeCanvas canvas, BeeEntity entity, RectangleF bounds)
        {
            var image = _rendererHelper.GetImageByRotation(entity.Rotation);
            canvas.DrawImage(image, bounds);
        }
    }
}
