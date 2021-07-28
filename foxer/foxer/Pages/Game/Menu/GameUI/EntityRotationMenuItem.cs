using System.Drawing;
using foxer.Core.Utils;
using foxer.Core.ViewModel.Menu;
using foxer.Render;
using foxer.Render.Menu;

namespace foxer.Pages.Game.Menu
{
    public class EntityRotationMenuItem : MenuItemBase
    {
        private readonly MenuButtonRenderer _button0 = new MenuButtonRenderer();
        private readonly MenuButtonRenderer _button90 = new MenuButtonRenderer();
        private readonly MenuButtonRenderer _button180 = new MenuButtonRenderer();
        private readonly MenuButtonRenderer _button270 = new MenuButtonRenderer();
        private readonly GameUIViewModel _viewModel;

        public EntityRotationMenuItem(GameUIViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        protected override void OnRender(INativeCanvas canvas, MenuItemInfoArgs args)
        {
            base.OnRender(canvas, args);

            if(!_viewModel.ShowRotationPanel())
            {
                return;
            }

            var cells = GeomUtils.SplitOnCells(args.Bounds, 3, 3);
            var size = cells[0, 0].Height / 4;
            _button0.Render(canvas, cells[2, 1], null, true, ActiveAnimation == _button0.WaitAfterClick);
            canvas.DrawText("0", cells[2, 1], Color.Cyan, size, HorAlign.Center);

            _button90.Render(canvas, cells[1, 0], null, true, ActiveAnimation == _button90.WaitAfterClick);
            canvas.DrawText("90", cells[1, 0], Color.Cyan, size, HorAlign.Center);

            _button180.Render(canvas, cells[0, 1], null, true, ActiveAnimation == _button180.WaitAfterClick);
            canvas.DrawText("180", cells[0, 1], Color.Cyan, size, HorAlign.Center);

            _button270.Render(canvas, cells[1, 2], null, true, ActiveAnimation == _button270.WaitAfterClick);
            canvas.DrawText("270", cells[1, 2], Color.Cyan, size, HorAlign.Center);
        }

        protected override bool OnTouch(PointF pt, MenuItemInfoArgs args)
        {
            if (!_viewModel.ShowRotationPanel())
            {
                return false;
            }

            var cells = GeomUtils.SplitOnCells(args.Bounds, 3, 3);
            if(cells[2, 1].Contains(pt))
            {
                _viewModel.SetItemRotation(0);
                StartAnimation(_button0.WaitAfterClick.Coroutine);
                return true;
            }
            else if (cells[1, 0].Contains(pt))
            {
                _viewModel.SetItemRotation(90);
                StartAnimation(_button90.WaitAfterClick.Coroutine);
                return true;
            }
            else if (cells[0, 1].Contains(pt))
            {
                _viewModel.SetItemRotation(180);
                StartAnimation(_button180.WaitAfterClick.Coroutine);
                return true;
            }
            else if (cells[1, 2].Contains(pt))
            {
                _viewModel.SetItemRotation(270);
                StartAnimation(_button270.WaitAfterClick.Coroutine);
                return true;
            }

            return false;
        }
    }
}
