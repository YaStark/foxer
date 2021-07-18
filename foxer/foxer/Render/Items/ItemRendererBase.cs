using foxer.Core.Game.Items;
using System.Drawing;

namespace foxer.Render.Items
{
    public abstract class ItemRendererBase<TItem> : IItemRenderer
        where TItem : ItemBase
    {
        private static readonly byte[] _counterBackground = Properties.Resources.cell_item_counter_bg;

        public bool CanRender<T>(T item)
        {
            return item.GetType() == typeof(TItem);
        }

        public void Render(INativeCanvas canvas, ItemBase item, RectangleF bounds, bool showCount)
        {
            Render(canvas, item as TItem, bounds, showCount);
        }
        
        protected abstract void Render(INativeCanvas canvas, TItem item, RectangleF bounds, bool showCount);

        protected void RenderCount(INativeCanvas canvas, int count, RectangleF initialBounds)
        {
            var newBounds = RectangleF.FromLTRB(
                initialBounds.Left + initialBounds.Width * 0.3f,
                initialBounds.Top + initialBounds.Height * 0.3f,
                initialBounds.Right,
                initialBounds.Bottom);
            canvas.DrawImage(_counterBackground, newBounds);
            newBounds.Inflate(-newBounds.Width / 4, -newBounds.Height / 4);
            canvas.DrawText(count.ToString(), newBounds, Color.Black);
        }
    }
}
