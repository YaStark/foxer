using System.Drawing;
using System.Linq;
using foxer.Core.Game.Items;
using foxer.Core.ViewModel.Menu.Craft;
using foxer.Pages.Game.Menu.Craft;
using foxer.Render.Menu;
using foxer.Render.Menu.Items;

namespace foxer.Pages.Game.Menu
{
    public class CraftMenu : MenuBase
    {
        public CraftMenu(MenuCraftViewModel viewModel)
            : base(viewModel, 12, 8)
        {
            // M I K + + + + + + + + +   M I K + + + + +
            // C S S S S D D 0 0 0 0 +   C S S S S D D +
            // + S S S S D D 0 0 0 0 +   + S S S S D D +
            // + S S S S D D 0 0 0 0 +   + S S S S D D +
            // + S S S S D D 0 0 0 0 +   + S S S S D D +
            // + S S S S R R 0 0 0 0 +   + S S S S R R +
            // + S S S S R R 0 0 0 0 +   + S S S S R R +
            // + + + + + + + + + + + +   + 0 0 0 0 0 0 +
            //                           + 0 0 0 0 0 0 +
            //                           + 0 0 0 0 0 0 +
            //                           + 0 0 0 0 0 0 +
            //                           + + + + + + + +

            BeginCreateCell(new MenuButton(viewModel.CommandCancel, Properties.Resources.icon_cancel)) // C
                .SetDefaultLayout(0, 1).End();

            BeginCreateCell(new MenuButton(viewModel.CommandOptions, Properties.Resources.icon_menu)) // M
                .SetDefaultLayout(0, 0).End();

            BeginCreateCell(new MenuButton(viewModel.CommandInventory, Properties.Resources.icon_inventory)) // I
                .SetDefaultLayout(1, 0).End();

            BeginCreateCell(new MenuButton(null, Properties.Resources.icon_craft)) // K
                .SetDefaultLayout(2, 0).End();

            var items = viewModel.CraftMenuManager.Crafter.Crafts
                .Select(cr => new CraftMenuListBoxItem(viewModel.CraftMenuManager, cr));

            var listBoxViewModel = new MenuListBoxViewModel(viewModel, items, new Size(4, 6));
            BeginCreateCell(new MenuListBox(listBoxViewModel)) // S
                .SetDefaultLayout(1, 1, 4, 6).End();

            BeginCreateCell(new CraftMenuDetails(viewModel.CraftMenuManager))// D
                .SetDefaultLayout(5, 1, 2, 4).End();

            BeginCreateCell(new CraftMenuResults(viewModel.CraftMenuManager))// R
                .SetDefaultLayout(5, 5, 2, 2).End();

            for (int i = 0; i < viewModel.FastPanel.Length; i++)
            {
                BeginCreateCell(MenuCell(viewModel, viewModel.FastPanel[i])) // 0
                    .SetDefaultLayout(i + 7, 1).SetTransponedLayout(1, i + 7).End();
            }

            for (int i = 0; i < viewModel.Inventory.GetLength(0); i++)
            {
                for (int j = 0; j < viewModel.Inventory.GetLength(1); j++)
                {
                    BeginCreateCell(MenuCell(viewModel, viewModel.Inventory[i, j])) // 0
                        .SetDefaultLayout(i + 7, j + 2).SetTransponedLayout(j + 2, i + 7).End();
                }
            }
        }

        private IMenuItem MenuCell(MenuCraftViewModel viewModel, IItemHolder itemHolder)
        {
            return new MenuCell(viewModel.InventoryManager, viewModel.ItemManager, itemHolder);
        }

        public override bool Touch(PointF pt, SizeF size)
        {
            base.Touch(pt, size);
            return true;
        }
    }
}
