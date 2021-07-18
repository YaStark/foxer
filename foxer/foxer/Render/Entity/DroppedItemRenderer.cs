using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Render.Items;

namespace foxer.Render
{
    public class DroppedItemRenderer : EntityRendererBase<DroppedItemEntity>
    {
        private static readonly ItemRendererFactory _itemRendererFactory = new ItemRendererFactory();

        protected override void Render(INativeCanvas canvas, DroppedItemEntity entity, RectangleF bounds)
        {
            if (entity.Item == null)
            {
                return;
            }

            var w2 = bounds.Width / 2;
            var h2 = bounds.Height / 2;
            canvas.Translate(
                bounds.Left + w2, 
                bounds.Top + h2);
            canvas.RotateDegrees(-45);

            var rect = RectangleF.FromLTRB(-w2, -h2, w2, h2);
            _itemRendererFactory.GetRenderer(entity.Item)
                ?.Render(canvas, entity.Item, rect, false);

            canvas.RotateDegrees(45);
            canvas.Translate(
                -bounds.Left - w2,
                -bounds.Top - h2);
        }
    }
}
