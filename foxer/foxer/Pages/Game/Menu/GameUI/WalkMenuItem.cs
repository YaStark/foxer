using System.Collections.Generic;
using System.Drawing;
using foxer.Core.ViewModel.Menu;
using foxer.Render;
using foxer.Render.Menu;
using foxer.Render.Menu.Animation;

namespace foxer.Pages.Game.Menu
{
    public class WalkMenuItem : MenuItemBase
    {
        private static readonly MenuCellRenderer _cellRenderer = new MenuCellRenderer();
        private static readonly byte[] _icon = Properties.Resources.icon_walk;
        private readonly GameUIViewModel _viewModel;

        public WalkMenuItem(GameUIViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        protected override bool OnTouch(PointF pt, MenuItemInfoArgs args)
        {
            StartAnimation(_cellRenderer.WaitAfterClick.Coroutine, Activate);
            return true;
        }

        private IEnumerable<UIAnimation> Activate(UICoroutineArgs args)
        {
            _viewModel.SetWalkMode();
            yield break;
        }

        protected override void OnRender(INativeCanvas canvas, MenuItemInfoArgs args)
        {
            base.OnRender(canvas, args);

            _cellRenderer.Render(
                canvas,
                args,
                ActiveAnimation == _cellRenderer.WaitAfterClick,
                _viewModel.WalkMode,
                false);

            canvas.DrawImage(_icon, args.Bounds);
        }
    }
}
