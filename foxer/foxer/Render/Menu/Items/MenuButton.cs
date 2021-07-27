using foxer.Render.Menu.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Input;

namespace foxer.Render.Menu
{
    public class MenuButton : MenuItemBase
    {
        private readonly MenuButtonRenderer _renderer = new MenuButtonRenderer();
        private readonly ICommand _cmd;
        private readonly byte[] _image;

        public bool Enabled { get; protected set; }
        
        public MenuButton(ICommand cmd, byte[] image)
        {
            _cmd = cmd;
            if(_cmd != null)
            {
                _cmd.CanExecuteChanged += OnCanExecuteChanged;
                Enabled = _cmd.CanExecute(null) == true;
            }

            _image = image;
        }

        protected override bool OnTouch(PointF pt, MenuItemInfoArgs args)
        {
            if (_cmd?.CanExecute(null) == true)
            {
                StartAnimation(_renderer.WaitAfterClick.Coroutine, Execute);
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
            if (args.Bounds.Size != args.CellSize)
            {
                _renderer.RenderBig(canvas, args.Bounds, args.CellSize, _image, Enabled, ActiveAnimation == _renderer.WaitAfterClick);
            }
            else
            {
                _renderer.Render(canvas, args.Bounds, _image, Enabled, ActiveAnimation == _renderer.WaitAfterClick);
            }
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            Enabled = _cmd?.CanExecute(null) == true;
        }
    }
}
