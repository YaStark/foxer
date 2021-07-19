using foxer.Core;
using foxer.Core.ViewModel.Menu;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Input;

namespace foxer.Render.Menu.Items
{
    public class MenuListBoxViewModel
    {
        private readonly DelegateCommand _commandScrollUp;
        private readonly DelegateCommand _commandScrollDown;
        private readonly GameMenuViewModelBase _viewModel;

        private int _scrollLinesIndex = 0;

        public Size ProposedSize { get; }
        public ICommand CommandScrollUp => _commandScrollUp;
        public ICommand CommandScrollDown => _commandScrollDown;

        public IReadOnlyList<MenuListBoxItemBase> Items { get; }

        public MenuListBoxViewModel(GameMenuViewModelBase viewModel, IEnumerable<MenuListBoxItemBase> items, Size proposedSize)
        {
            _viewModel = viewModel;
            Items = items.ToArray();
            ProposedSize = proposedSize;
            _commandScrollUp = new DelegateCommand(OnScrollUp, NeedShowScroll());
            _commandScrollDown = new DelegateCommand(OnScrollDown, NeedShowScroll());
        }

        public bool Touch(int i, int j, PointF pt, MenuItemInfoArgs args)
        {
            int width = NeedShowScroll() ? ProposedSize.Width - 1 : ProposedSize.Width;
            int k = (_scrollLinesIndex + j) * width + i;
            var visible = Items.Where(item => item.GetVisible(args.Stage)).ToArray();
            if (k >= visible.Length) return false;
            return visible[k].Touch(pt, args);
        }

        private void OnScrollDown()
        {
            if(!NeedShowScroll())
            {
                _commandScrollDown.Enabled = false;
                _commandScrollUp.Enabled = false;
                return;
            }

            int width = ProposedSize.Width - 1;
            int cellsCount = width * ProposedSize.Height;
            int visibleItemsCount = Items.Count(i => i.GetVisible(_viewModel.Stage));
            _scrollLinesIndex += 2;
            if (_scrollLinesIndex * width + cellsCount == visibleItemsCount)
            {
                _commandScrollDown.Enabled = false;
                _commandScrollUp.Enabled = _scrollLinesIndex > 0;
            }
            else if ((_scrollLinesIndex - 1) * width + cellsCount > visibleItemsCount)
            {
                _scrollLinesIndex = (visibleItemsCount - cellsCount) / width + 1;
                _commandScrollDown.Enabled = false;
                _commandScrollUp.Enabled = _scrollLinesIndex > 0;
            }
            else
            {
                _commandScrollDown.Enabled = true;
                _commandScrollUp.Enabled = true;
            }
        }

        private void OnScrollUp()
        {
            if (!NeedShowScroll())
            {
                _commandScrollDown.Enabled = false;
                _commandScrollUp.Enabled = false;
                return;
            }

            int width = ProposedSize.Width - 1;
            int cellsCount = width * ProposedSize.Height;
            _scrollLinesIndex = Math.Max(_scrollLinesIndex - 2, 0);
            _commandScrollUp.Enabled = _scrollLinesIndex > 0;
            _commandScrollDown.Enabled = true;
        }

        public bool NeedShowScroll()
        {
            return ProposedSize.Width * ProposedSize.Height < Items.Count(i => i.GetVisible(_viewModel.Stage));
        }
        
        public IReadOnlyList<MenuListBoxItemBase> GetItems()
        {
            if (!NeedShowScroll())
            {
                return Items.Where(i => i.GetVisible(_viewModel.Stage)).ToArray();
            }

            int width = ProposedSize.Width - 1;
            int cellsCount = width * ProposedSize.Height;
            var items = Items.Where(i => i.GetVisible(_viewModel.Stage)).ToArray();
            int takeCount = cellsCount;
            if ((_scrollLinesIndex - 1) * width + cellsCount > items.Length)
            {
                _scrollLinesIndex = (items.Length - cellsCount) / width + 1;
                takeCount = items.Length - (_scrollLinesIndex - 1) * width;
            }

            return Items.Skip(_scrollLinesIndex * width).Take(takeCount).ToArray();
        }
    }
}
