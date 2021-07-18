using foxer.Core.ViewModel;

namespace foxer.Core.Game.Items
{
    public class FastPanelItemHolder : IItemHolder
    {
        private readonly PageGameViewModel _viewModel;
        private readonly int _index;

        public FastPanelItemHolder(PageGameViewModel viewModel, int i)
        {
            _viewModel = viewModel;
            _index = i;
        }

        public void Clear()
        {
            Set(null);
        }

        public ItemBase Get()
        {
            return _viewModel.Stage.InventoryManager.GetFastPanel(_index);
        }

        public void Set(ItemBase item)
        {
            _viewModel.Stage.InventoryManager.SetFastPanel(_index, item);
        }
    }
}
