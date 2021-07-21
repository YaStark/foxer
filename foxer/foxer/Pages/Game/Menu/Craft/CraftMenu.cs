using System.Drawing;
using System.Linq;
using foxer.Core.Game.Items;
using foxer.Core.ViewModel.Menu.Craft;
using foxer.Pages.Game.Menu.Craft;
using foxer.Render.Menu;
using foxer.Render.Menu.Items;

namespace foxer.Pages.Game.Menu
{
    public class CraftMenu : InventoryMenuBase
    {
        public CraftMenu(MenuCraftViewModel viewModel)
            : base(viewModel, viewModel.InventoryManager, "Craft")
        {
            // M I K + + + + + + + + +   M I K + + + + +
            // C S S S S D D 0 0 0 0 +   C S S S S D D +
            // + S S S S D D 0 0 0 0 +   + S S S S D D +
            // + S S S S D D 0 0 0 0 +   + S S S S D D +
            // + S S S S D D 0 0 0 0 +   + S S S S D D +
            // + S S S S R R 0 0 0 0 +   + S S S S R R +
            // + S S S S R R 0 0 0 0 +   + S S S S R R +
            // + + + + + + + V V V V +   + 0 0 0 0 0 0 +
            //                           + 0 0 0 0 0 0 +
            //                           + 0 0 0 0 0 0 +
            //                           + 0 0 0 0 0 0 +
            //                           + + + V V V V +

            var items = viewModel.CraftMenuManager.Crafter.Crafts
                .Select(cr => new CraftMenuListBoxItem(viewModel.CraftMenuManager, cr));

            var listBoxViewModel = new MenuListBoxViewModel(viewModel, items, new Size(4, 6));
            BeginCreateCell(new MenuListBox(listBoxViewModel)) // S
                .SetDefaultLayout(1, 1, 4, 6).End();

            BeginCreateCell(new CraftMenuDetails(viewModel.CraftMenuManager))// D
                .SetDefaultLayout(5, 1, 2, 4).End();

            BeginCreateCell(new CraftMenuResults(viewModel.CraftMenuManager))// R
                .SetDefaultLayout(5, 5, 2, 2).End();
        }

        public override bool Touch(PointF pt, SizeF size)
        {
            base.Touch(pt, size);
            return true;
        }
    }
}
