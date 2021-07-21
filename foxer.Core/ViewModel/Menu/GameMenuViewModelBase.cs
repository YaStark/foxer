using foxer.Core.Game;
using foxer.Core.Game.Info;
using foxer.Core.Game.Items;
using System.Windows.Input;

namespace foxer.Core.ViewModel.Menu
{
    public abstract class GameMenuViewModelBase : IItemInfoProviderFactory
    {
        private readonly DelegateCommand _commandCloseMenu;
        private readonly DelegateCommand _commandOptions;
        private readonly DelegateCommand _commandInventory;
        private readonly DelegateCommand _commandCraft;

        protected PageGameViewModel ViewModel { get; }

        public IItemHolder[,] Inventory { get; }

        public IItemHolder[] FastPanel { get; }

        public Stage Stage => ViewModel.Stage;
        
        public ICommand CommandCancel => _commandCloseMenu;

        public ICommand CommandOptions => _commandOptions;

        public ICommand CommandCraft => _commandCraft;

        public ICommand CommandInventory => _commandInventory;

        public ItemManager ItemManager => ViewModel.ItemManager;

        public ItemInfoManager ItemInfoManager => ViewModel.ItemInfoManager;

        public IItemInfoProviderFactory ItemInfoProviderFactory => this;

        protected GameMenuViewModelBase(PageGameViewModel viewModel)
        {
            ViewModel = viewModel;

            _commandCloseMenu = new DelegateCommand(() => ViewModel.GameMenu.CloseMenu(), true);
            _commandOptions = new DelegateCommand(() => ViewModel.GameMenu.OpenOptionsMenu(), true);
            _commandInventory = new DelegateCommand(() => ViewModel.GameMenu.OpenInventoryMenu(), true);
            _commandCraft = new DelegateCommand(() => ViewModel.GameMenu.OpenCraftMenu(), true);

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
        }

        public virtual IItemInfoProvider GetItemInfoProvider()
        {
            var item = GetItem();
            if(item != null
                && ViewModel.ItemInfoManager.TryGetByObject(item, out var provider))
            {
                return provider;
            }

            return null;
        }
        
        public virtual object GetItem()
        {
            return null;
        }
    }
}
