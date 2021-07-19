using foxer.Core.Utils;
using foxer.Render.Menu.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Input;

namespace foxer.Render.Menu
{
    public class MenuButton : MenuItemBase
    {
        private static readonly RectangleF[,] _spriteMask = GeomUtils.CreateSpriteMask3x3(0.45f, 0.1f, 0.45f, 0.1f);
        private static readonly byte[] _imageNormal;
        private static readonly byte[] _imageHovered;
        private static readonly byte[] _imageDisabled;

        private readonly ICommand _cmd;
        private readonly byte[] _image;

        public UIAnimation WaitAfterClick { get; }

        public bool Enabled { get; protected set; }
        
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

        protected override bool OnTouch(PointF pt, MenuItemInfoArgs args)
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

        protected override void OnRender(INativeCanvas canvas, MenuItemInfoArgs args)
        {
            base.OnRender(canvas, args);

            var bounds = args.Bounds;
            if(args.Bounds.Size != args.CellSize)
            {
                RenderSprite3x3(canvas, GetImage(), _spriteMask, args.Bounds, args.CellSize);
                if(_image != null)
                {
                    canvas.DrawImage(_image, GeomUtils.DeflateTo(bounds, args.CellSize));
                }
            }
            else
            {
                canvas.DrawImage(GetImage(), bounds);
                if(_image != null)
                {
                    bounds.Inflate(-bounds.Width * 0.2f, -bounds.Height * 0.2f);
                    canvas.DrawImage(_image, bounds);
                }
            }
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            Enabled = _cmd?.CanExecute(null) == true;
        }

        private byte[] GetImage()
        {
            return Enabled 
                ? ActiveAnimation == WaitAfterClick 
                    ? _imageHovered 
                    : _imageNormal
                : _imageDisabled;
        }
    }
}
