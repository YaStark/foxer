using System.Drawing;
using foxer.Core.Utils;

namespace foxer.Render.Menu.Items
{
    public class MenuListBox : MenuItemBase
    {
        private readonly MenuListBoxViewModel _viewModel;
        private static readonly byte[] _imageScrollUp = Properties.Resources.icon_up;
        private static readonly byte[] _imageScrollDown = Properties.Resources.icon_down;

        private readonly MenuButton _buttonScrollUp;
        private readonly MenuButton _buttonScrollDown;

        public MenuListBox(MenuListBoxViewModel viewModel)
        {
            _viewModel = viewModel;
            _buttonScrollUp = new MenuButton(_viewModel.CommandScrollUp, _imageScrollUp);
            _buttonScrollDown = new MenuButton(_viewModel.CommandScrollDown, _imageScrollDown);
        }

        protected override bool OnTouch(PointF pt, MenuItemInfoArgs args)
        {
            var cs = args.CellSize;
            var bounds = GeomUtils.Deflate(args.Bounds, cs.Width * 0.2f, cs.Height * 0.2f);
            var width = _viewModel.ProposedSize.Width;
            int height = _viewModel.ProposedSize.Height;
            var cells = GeomUtils.SplitOnCells(bounds, width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (!cells[i, j].Contains(pt))
                    {
                        continue;
                    }

                    int width0 = width;
                    if(_viewModel.NeedShowScroll())
                    {
                        if (i == width - 1)
                        {
                            // scroll buttons
                            if (j == 0) return _buttonScrollUp.Touch(pt, args);
                            if (j == height - 1) return _buttonScrollUp.Touch(pt, args);
                            return false;
                        }

                        width0--;
                    }

                    return _viewModel.Touch(i, j, pt, args);
                }
            }

            return base.OnTouch(pt, args);
        }

        protected override void OnRender(INativeCanvas canvas, MenuItemInfoArgs args)
        {
            base.OnRender(canvas, args);

            var cs = args.CellSize;
            RenderBackground(canvas, args.Bounds, cs);

            var bounds = GeomUtils.Deflate(args.Bounds, cs.Width * 0.2f, cs.Height * 0.2f);
            var width = _viewModel.ProposedSize.Width;
            int height = _viewModel.ProposedSize.Height;
            var cells = GeomUtils.SplitOnCells(bounds, width, height);
            if (_viewModel.NeedShowScroll())
            {
                _buttonScrollUp.Render(
                    canvas, 
                    new MenuItemInfoArgs(args.Stage, cells[width - 1, 0], cells[width - 1, 0].Size));

                _buttonScrollDown.Render(
                    canvas, 
                    new MenuItemInfoArgs(args.Stage, cells[width - 1, height - 1], cells[width - 1, height - 1].Size));

                // todo render scroll bar
                width--;
            }

            // render items
            var items = _viewModel.GetItems();
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                for (int i = 0; i < width; i++)
                {
                    int k = i + j * width;
                    if (k >= items.Count) break;
                    items[k].Render(
                        canvas,
                        new MenuItemInfoArgs(args.Stage, cells[i, j], cells[i, j].Size));
                }
            }
        }
    }
}
