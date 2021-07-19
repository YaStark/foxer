using foxer.Core.Utils;
using foxer.Core.ViewModel;
using foxer.Core.ViewModel.Menu;
using foxer.Render;
using foxer.Render.Menu;

namespace foxer.Pages.Game.Menu
{
    public class MenuHost : IMenuHost
    {
        private readonly IMenu _inventoryMenu;
        private readonly IMenu _optionsMenu;
        private readonly IMenu _craftMenu;
        private readonly IMenu _gameUI;

        public IMenu Menu { get; private set; }

        public MenuHost(PageGameViewModel viewModel)
        {
            _inventoryMenu = new InventoryMenu(viewModel.MenuInventory);
            _optionsMenu = new OptionsMenu(viewModel.MenuOptions);
            _craftMenu = new CraftMenu(viewModel.MenuCraft);
            _gameUI = new GameUI(viewModel.GameUI);
            CloseMenu();
        }

        public void CloseMenu()
        {
            Menu = _gameUI;
        }

        public bool EnsureMenuClosed()
        {
            return Menu == _gameUI;
        }

        public void OpenInventoryMenu()
        {
            Menu = _inventoryMenu;
        }

        public void OpenOptionsMenu()
        {
            Menu = _optionsMenu;
        }

        public void OpenCraftMenu()
        {
            Menu = _craftMenu;
        }
    }
}
