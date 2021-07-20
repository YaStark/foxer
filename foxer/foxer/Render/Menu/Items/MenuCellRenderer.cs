using foxer.Render.Items;
using foxer.Render.Menu.Animation;

namespace foxer.Render.Menu
{
    public class MenuCellRenderer
    {
        private static readonly byte[] _imageNormal = Properties.Resources.cell_normal;
        private static readonly byte[] _imageHovered = Properties.Resources.cell_hovered;
        private static readonly byte[] _imageDisabled = Properties.Resources.cell_disabled;
        private static readonly byte[] _imageSelected = Properties.Resources.cell_selected;
        private static readonly byte[] _imageActive = Properties.Resources.cell_active;

        private static readonly ItemRendererFactory _itemRendererFactory = new ItemRendererFactory();

        public UIAnimation WaitAfterClick { get; }

        public MenuCellRenderer()
        {
            WaitAfterClick = new UIWaitingAnimation(50);
        }

        public void Render(INativeCanvas canvas, MenuItemInfoArgs args, bool hovered, bool active, bool selected)
        {
            var bounds = args.Bounds;
            canvas.DrawImage(GetImage(hovered, active), bounds);
            if (selected)
            {
                canvas.DrawImage(_imageSelected, bounds);
            }
        }

        private byte[] GetImage(bool hovered, bool active)
        {
            if (hovered)
            {
                return _imageHovered;
            }

            if (active)
            {
                return _imageActive;
            }

            return _imageNormal;
        }
    }
}
