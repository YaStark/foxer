using foxer.Core.ViewModel;

namespace foxer.Core.Game.Items
{
    public class InventoryItemHolder : IItemHolder
    {
        private readonly PageGameViewModel _viewModel;
        private readonly int _x;
        private readonly int _y;

        public InventoryItemHolder(PageGameViewModel viewModel, int x, int y)
        {
            _viewModel = viewModel;
            _x = x;
            _y = y;
        }

        public void Clear()
        {
            Set(null);
        }

        public ItemBase Get()
        {
            return _viewModel.Stage.InventoryManager.GetInventory(_x, _y);
        }

        public void Set(ItemBase item)
        {
            _viewModel.Stage.InventoryManager.SetInventory(_x, _y, item);
        }
    }
}
