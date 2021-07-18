using System.Windows.Input;

namespace foxer.Core.ViewModel.Menu
{
    public abstract class GameMenuViewModelBase
    {
        protected PageGameViewModel ViewModel { get; }

        public ICommand CommandCancel => ViewModel.CommandCloseMenu;

        public ICommand CommandOptions => ViewModel.CommandOptions;

        public ICommand CommandCraft => ViewModel.CommandCraft;

        public ICommand CommandInventory => ViewModel.CommandInventory;

        protected GameMenuViewModelBase(PageGameViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
