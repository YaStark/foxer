using System.Drawing;
using foxer.Core.ViewModel.Menu.Craft;
using foxer.Render.Menu;

namespace foxer.Pages.Game.Menu
{
    public class CraftMenu : MenuBase
    {
        public CraftMenu(MenuCraftViewModel viewModel)
            : base(10, 6)
        {
            // M I K + + + + + + +   M I K + + +
            // C + + + + 0 0 0 0 +   C + + + + +
            // + + + + + 0 0 0 0 +   + + + + + +
            // + + + + + + 0 0 0 +   + + + + + +
            // + + + + + + 0 0 0 +   + + + + + +
            // + + + + + + + + + +   + 0 0 + + +
            //                       + 0 0 0 0 +
            //                       + 0 0 0 0 +
            //                       + 0 0 0 0 +
            //                       + + + + + +

            BeginCreateCell(new MenuButton(viewModel.CommandCancel, Properties.Resources.icon_cancel)) // C
                .SetDefaultLayout(0, 1).End();

            BeginCreateCell(new MenuButton(viewModel.CommandOptions, Properties.Resources.icon_menu)) // M
                .SetDefaultLayout(0, 0).End();

            BeginCreateCell(new MenuButton(viewModel.CommandInventory, Properties.Resources.icon_inventory)) // I
                .SetDefaultLayout(1, 0).End();

            BeginCreateCell(new MenuButton(null, Properties.Resources.icon_craft)) // K
                .SetDefaultLayout(2, 0).End();

            for (int i = 0; i < viewModel.Inventory.GetLength(0); i++)
            {
                for (int j = 0; j < viewModel.Inventory.GetLength(1); j++)
                {
                    BeginCreateCell(new MenuCell(viewModel.InventoryManager, viewModel.Inventory[i, j])) // 0
                        .SetDefaultLayout(i + 6, j + 1).SetTransponedLayout(j + 1, i + 6).End();
                }
            }
        }

        public override bool Touch(PointF pt, SizeF size)
        {
            base.Touch(pt, size);
            return true;
        }
    }
}
