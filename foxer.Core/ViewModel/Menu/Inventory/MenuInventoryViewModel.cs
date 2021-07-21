using foxer.Core.Game.Items;

namespace foxer.Core.ViewModel.Menu
{
    public class MenuInventoryViewModel : GameMenuViewModelBase
    {
        private readonly MovingItemsInventoryManager _inventoryManager;

        public IInventoryManager InventoryManager => _inventoryManager;

        public MenuInventoryViewModel(PageGameViewModel viewModel)
            : base(viewModel)
        {
            _inventoryManager = new MovingItemsInventoryManager(viewModel, FastPanel);
        }

        public override object GetItem()
        {
            return _inventoryManager.Selected?.Get();
        }
    }
}
