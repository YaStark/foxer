using System;
using foxer.Core.Game.Craft;
using foxer.Core.Game.Info;

namespace foxer.Core.ViewModel.Menu.Craft
{
    public class MenuCraftViewModel : GameMenuViewModelBase, ICraftMenuManager
    {
        private readonly CraftInventoryManager _craftInventoryManager;

        private ItemCraftBase _selectedCraft;
        private object _lastSelectedItem;

        public IInventoryManager InventoryManager => _craftInventoryManager;

        public ICraftMenuManager CraftMenuManager => this;

        public ItemCraftBase SelectedCraft
        {
            get { return _selectedCraft; }
            set {
                _selectedCraft = value;
                _lastSelectedItem = _selectedCraft;
            }
        }

        public CrafterBase Crafter => ViewModel.PlayerHandsCrafter;

        public MenuCraftViewModel(PageGameViewModel viewModel) 
            : base(viewModel)
        {
            _craftInventoryManager = new CraftInventoryManager(viewModel, FastPanel);
            _craftInventoryManager.SelectedChanged += OnInventoryItemSelectedChanged;
        }

        public override object GetItem()
        {
            return _lastSelectedItem;
        }
        
        public void SetSelectedRequirement(CraftRequirementsBase requirement)
        {
            _lastSelectedItem = requirement;
        }

        private void OnInventoryItemSelectedChanged(object sender, EventArgs e)
        {
            if(_craftInventoryManager.Selected != null)
            {
                _lastSelectedItem = _craftInventoryManager.Selected.Get();
            }
        }
    }
}
