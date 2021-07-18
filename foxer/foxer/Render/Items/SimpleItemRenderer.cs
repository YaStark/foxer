using foxer.Core.Game.Items;
using System.Drawing;

namespace foxer.Render.Items
{
    public class SimpleItemRenderer<TItem> : ItemRendererBase<TItem>
        where TItem : ItemBase
    {
        private readonly byte[] _image;

        public SimpleItemRenderer(byte[] image)
        {
            _image = image;
        }

        protected override void Render(INativeCanvas canvas, TItem item, RectangleF bounds, bool showCount)
        {
            canvas.DrawImage(_image, bounds);
            if (item.CanStack && showCount)
            {
                RenderCount(canvas, item?.Count ?? 0, bounds);
            }
        }
    }
}
