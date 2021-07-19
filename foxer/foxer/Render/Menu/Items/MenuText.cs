using System.Drawing;

namespace foxer.Render.Menu
{
    public class MenuText : MenuItemBase
    {
        private readonly string _text;
        private readonly Color _textColor;
        private readonly bool _showBackground;

        public MenuText(string text, Color textColor, bool showBackground)
        {
            _text = text;
            _textColor = textColor;
            _showBackground = showBackground;
        }

        protected override void OnRender(INativeCanvas canvas, MenuItemInfoArgs args)
        {
            base.OnRender(canvas, args);

            if(_showBackground)
            {
                RenderBackground(canvas, args.Bounds, args.CellSize);
            }

            canvas.DrawText(_text, args.Bounds, _textColor);
        }
    }
}
