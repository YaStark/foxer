using foxer.Core.Utils;
using foxer.Render.Menu.Animation;
using System.Drawing;

namespace foxer.Render.Menu
{
    public class MenuButtonRenderer
    {
        private static readonly RectangleF[,] _spriteMask = GeomUtils.CreateSpriteMask3x3(0.45f, 0.1f, 0.45f, 0.1f);
        private static readonly byte[] _imageNormal = Properties.Resources.button_normal;
        private static readonly byte[] _imageHovered = Properties.Resources.button_hovered;
        private static readonly byte[] _imageDisabled = Properties.Resources.button_disabled;

        public UIAnimation WaitAfterClick { get; }

        public MenuButtonRenderer()
        {
            WaitAfterClick = new UIWaitingAnimation(50);
        }

        public void Render(INativeCanvas canvas, RectangleF bounds, byte[] icon, bool enabled, bool hovered)
        {
            canvas.DrawImage(GetImage(enabled, hovered), bounds);
            if (icon != null)
            {
                bounds.Inflate(-bounds.Width * 0.2f, -bounds.Height * 0.2f);
                canvas.DrawImage(icon, bounds);
            }
        }

        public void RenderBig(INativeCanvas canvas, RectangleF bounds, SizeF cellSize, byte[] icon, bool enabled, bool hovered)
        {
            MenuItemBase.RenderSprite3x3(canvas, GetImage(enabled, hovered), _spriteMask, bounds, cellSize);
            if (icon != null)
            {
                canvas.DrawImage(icon, GeomUtils.DeflateTo(bounds, cellSize));
            }
        }

        private byte[] GetImage(bool enabled, bool hovered)
        {
            return enabled
                ? hovered
                    ? _imageHovered
                    : _imageNormal
                : _imageDisabled;
        }
    }
}
