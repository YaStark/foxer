using foxer.Render.Menu.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Input;

namespace foxer.Render.Menu
{
    public class MenuButton : MenuItemBase
    {
        private static readonly byte[] _imageNormal;
        private static readonly byte[] _imageHovered;
        private static readonly byte[] _imageDisabled;

        private readonly ICommand _cmd;
        private readonly byte[] _image;

        public UIAnimation WaitAfterClick { get; }

        public bool Enabled { get; private set; }
        
        static MenuButton()
        {
            _imageNormal = Properties.Resources.button_normal;
            _imageHovered = Properties.Resources.button_hovered;
            _imageDisabled = Properties.Resources.button_disabled;
        }

        public MenuButton(ICommand cmd, byte[] image)
        {
            _cmd = cmd;
            if(_cmd != null)
            {
                _cmd.CanExecuteChanged += OnCanExecuteChanged;
                Enabled = _cmd.CanExecute(null) == true;
            }

            _image = image;

            WaitAfterClick = new UIWaitingAnimation(50);
        }

        protected override bool OnTouch(PointF pt, RectangleF bounds)
        {
            if (_cmd?.CanExecute(null) == true)
            {
                StartAnimation(WaitAfterClick.Coroutine, Execute);
            }

            return true;
        }

        private IEnumerable<UIAnimation> Execute(UICoroutineArgs args)
        {
            if(_cmd?.CanExecute(null) == true)
            {
                _cmd.Execute(null);
            }

            yield return null;
        }

        protected override void OnRender(INativeCanvas canvas, RectangleF bounds)
        {
            base.OnRender(canvas, bounds);
            if(Enabled)
            {
                canvas.DrawImage(ActiveAnimation == WaitAfterClick ? _imageHovered : _imageNormal, bounds);
            }
            else
            {
                canvas.DrawImage(_imageDisabled, bounds);
            }

            bounds.Inflate(-bounds.Width * 0.2f, -bounds.Height * 0.2f);
            canvas.DrawImage(_image, bounds);
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            Enabled = _cmd?.CanExecute(null) == true;
        }
    }
}
