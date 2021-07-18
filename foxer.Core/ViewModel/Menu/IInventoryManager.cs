using foxer.Core.Game.Items;

namespace foxer.Core.ViewModel.Menu
{
    public interface IInventoryManager
    {
        bool GetSelected(IItemHolder itemHolder);
        bool GetActive(IItemHolder itemHolder);
        void SetSelected(IItemHolder itemHolder);
    }
}
