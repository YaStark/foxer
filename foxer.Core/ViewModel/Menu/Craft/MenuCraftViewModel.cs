using foxer.Core.Game.Craft;
using foxer.Core.Game.Items;

namespace foxer.Core.ViewModel.Menu.Craft
{
    public class MenuCraftViewModel : GameMenuViewModelBase, ICraftMenuManager
    {
        public IItemHolder[] FastPanel { get; }
        public IItemHolder[,] Inventory { get; }

        public IInventoryManager InventoryManager { get; }
        public ICraftMenuManager CraftMenuManager => this;

        ItemCraftBase ICraftMenuManager.Selected { get;set; }

        public CrafterBase Crafter => ViewModel.PlayerHandsCrafter;

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

            FastPanel = new IItemHolder[ViewModel.FastPanelSize];
            for (int i = 0; i < FastPanel.Length; i++)
            {
                FastPanel[i] = new FastPanelItemHolder(viewModel, i);
            }

            InventoryManager = new CraftInventoryManager(viewModel, FastPanel);
        }
    }
}
