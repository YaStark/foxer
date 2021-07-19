using foxer.Core.Game.Items;
using System.Drawing;

namespace foxer.Render.Items
{
    public interface IItemRenderer
    {
        void Render(INativeCanvas canvas, ItemBase item, RectangleF bounds, bool showCount);
        void RenderForCraft(INativeCanvas canvas, string count, RectangleF bounds, bool showCount);
    }
}
