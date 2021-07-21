using foxer.Core.ViewModel.Menu;
using System.Drawing;

namespace foxer.Pages.Game.Menu
{
    public class InventoryMenu : InventoryMenuBase
    {
        public InventoryMenu(MenuInventoryViewModel viewModel) 
            : base(viewModel, viewModel.InventoryManager, "Inventory")
        {
        }

        public override bool Touch(PointF pt, SizeF size)
        {
            base.Touch(pt, size);
            return true;
        }
    }
}
