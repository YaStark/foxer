using foxer.Core.Game.Items;

namespace foxer.Core.ViewModel.Menu
{
    public class MenuInventoryViewModel : GameMenuViewModelBase
    {
        public IItemHolder[,] Inventory { get; }

        public IItemHolder Selected { get; private set; }

        public IItemHolder[] FastPanel { get; private set; }

        public IInventoryManager InventoryManager { get; }

        public MenuInventoryViewModel(PageGameViewModel viewModel)
            : base(viewModel)
        {
            Inventory = new IItemHolder[ViewModel.InventorySize.Width, ViewModel.InventorySize.Height];
            for (int i = 0; i < ViewModel.InventorySize.Width; i++)
            {
                for (int j = 0; j < ViewModel.InventorySize.Height; j++)
                {
                    Inventory[i, j] = new InventoryItemHolder(viewModel, i, j);
                }
            }

            FastPanel = new IItemHolder[viewModel.FastPanelSize];
            for (int i = 0; i < viewModel.FastPanelSize; i++)
            {
                FastPanel[i] = new FastPanelItemHolder(viewModel, i);
            }

            InventoryManager = new MovingItemsInventoryManager(viewModel, FastPanel);
        }
    }
}
