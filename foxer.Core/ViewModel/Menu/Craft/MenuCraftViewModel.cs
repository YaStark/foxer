using foxer.Core.Game.Items;

namespace foxer.Core.ViewModel.Menu.Craft
{
    public class MenuCraftViewModel : GameMenuViewModelBase, IInventoryManager
    {
        public IItemHolder[,] Inventory { get; }

        public IInventoryManager InventoryManager => this;

        public MenuCraftViewModel(PageGameViewModel viewModel) 
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
        }

        public bool GetActive(IItemHolder itemHolder)
        {
            return false; // todo
        }

        public bool GetSelected(IItemHolder itemHolder)
        {
            return false; // todo
        }

        public void SetSelected(IItemHolder itemHolder)
        {
            // todo
        }
    }
}
