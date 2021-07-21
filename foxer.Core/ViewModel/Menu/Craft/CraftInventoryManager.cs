using foxer.Core.Game.Items;
using System;

namespace foxer.Core.ViewModel.Menu.Craft
{
    public class CraftInventoryManager : IInventoryManager
    {
        private readonly MovingItemsInventoryManager _movingItemsInventoryManager;

        public event EventHandler SelectedChanged
        {
            add { _movingItemsInventoryManager.SelectedChanged += value; }
            remove { _movingItemsInventoryManager.SelectedChanged -= value; }
        }

        public IItemHolder Selected => _movingItemsInventoryManager.Selected;

        public CraftInventoryManager(PageGameViewModel viewModel, IItemHolder[] fastPanel)
        {
            _movingItemsInventoryManager = new MovingItemsInventoryManager(viewModel, fastPanel);
            // todo : add filter recipes
        }

        public bool GetActive(IItemHolder itemHolder)
        {
            return _movingItemsInventoryManager.GetActive(itemHolder);
        }

        public bool GetSelected(IItemHolder itemHolder)
        {
            return _movingItemsInventoryManager.GetSelected(itemHolder);
        }

        public void SetSelected(IItemHolder itemHolder)
        {
            _movingItemsInventoryManager.SetSelected(itemHolder);
        }
    }
}
