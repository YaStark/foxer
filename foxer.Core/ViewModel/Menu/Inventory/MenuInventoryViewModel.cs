using foxer.Core.Game.Items;

namespace foxer.Core.ViewModel.Menu
{
    public class MenuInventoryViewModel : GameMenuViewModelBase, IInventoryManager
    {
        public IItemHolder[,] Inventory { get; }

        public IItemHolder Selected { get; private set; }

        public IItemHolder[] FastPanel { get; private set; }

        public IInventoryManager InventoryManager => this;

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
        }

        public void SetSelected(IItemHolder itemHolder)
        {
            if (Selected == null)
            {
                Selected = itemHolder;
                return;
            }

            if (Selected == itemHolder)
            {
                Selected = null;
                return;
            }

            var item = Selected.Get();
            Selected.Set(itemHolder.Get());
            itemHolder.Set(item);

            if(FastPanel[ViewModel.FastPanelSelectedIndex] == itemHolder)
            {
                ViewModel.SetActiveItem(itemHolder.Get());
            }
            else if(FastPanel[ViewModel.FastPanelSelectedIndex] == Selected)
            {
                ViewModel.SetActiveItem(Selected.Get());
            }

            Selected = null;
        }

        public bool GetSelected(IItemHolder itemHolder)
        {
            return itemHolder == Selected && itemHolder != null;
        }

        public bool GetActive(IItemHolder itemHolder)
        {
            return ViewModel.FastPanelSelectedIndex >= 0
                && ViewModel.FastPanelSelectedIndex < FastPanel.Length
                && FastPanel[ViewModel.FastPanelSelectedIndex] == itemHolder;
        }
    }
}
