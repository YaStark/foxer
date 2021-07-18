using System.Drawing;

namespace foxer.Render.Menu
{
    public class MenuText : MenuItemBase
    {
        private readonly string _text;
        private readonly Color _textColor;

        public MenuText(string text, Color textColor)
        {
            _text = text;
            _textColor = textColor;
        }

        protected override void OnRender(INativeCanvas canvas, RectangleF bounds)
        {
            base.OnRender(canvas, bounds);
            canvas.DrawText(_text, bounds, _textColor);
        }
    }
}
