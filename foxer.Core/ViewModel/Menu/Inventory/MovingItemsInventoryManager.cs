using foxer.Core.Game.Items;

namespace foxer.Core.ViewModel.Menu
{
    public class MovingItemsInventoryManager : IInventoryManager
    {
        private readonly PageGameViewModel _viewModel;
        private readonly IItemHolder[] _fastPanel;

        public IItemHolder Selected { get; private set; }

        public MovingItemsInventoryManager(PageGameViewModel viewModel, IItemHolder[] fastPanel)
        {
            _viewModel = viewModel;
            _fastPanel = fastPanel;
        }

        public bool GetActive(IItemHolder itemHolder)
        {
            return _viewModel.FastPanelSelectedIndex >= 0
                && _viewModel.FastPanelSelectedIndex < _fastPanel.Length
                && _fastPanel[_viewModel.FastPanelSelectedIndex] == itemHolder;
        }

        public bool GetSelected(IItemHolder itemHolder)
        {
            return itemHolder == Selected && itemHolder != null;
        }

        public void SetSelected(IItemHolder itemHolder)
        {
            if (Selected == null)
            {
                Selected = itemHolder;
                return;
            }

            if (Selected == itemHolder)
            {
                Selected = null;
                return;
            }

            var item = Selected.Get();
            Selected.Set(itemHolder.Get());
            itemHolder.Set(item);

            if (_fastPanel[_viewModel.FastPanelSelectedIndex] == itemHolder)
            {
                _viewModel.SetActiveItem(itemHolder.Get());
            }
            else if (_fastPanel[_viewModel.FastPanelSelectedIndex] == Selected)
            {
                _viewModel.SetActiveItem(Selected.Get());
            }

            Selected = null;
        }
    }
}
