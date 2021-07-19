using foxer.Core.Game;
using foxer.Core.Game.Items;
using System.Windows.Input;

namespace foxer.Core.ViewModel.Menu
{
    public abstract class GameMenuViewModelBase
    {
        protected PageGameViewModel ViewModel { get; }

        public Stage Stage => ViewModel.Stage;
        
        public ICommand CommandCancel => ViewModel.CommandCloseMenu;

        public ICommand CommandOptions => ViewModel.CommandOptions;

        public ICommand CommandCraft => ViewModel.CommandCraft;

        public ICommand CommandInventory => ViewModel.CommandInventory;

        public ItemManager ItemManager => ViewModel.ItemManager;

        protected GameMenuViewModelBase(PageGameViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
