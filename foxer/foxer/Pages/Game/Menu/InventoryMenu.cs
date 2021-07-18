using foxer.Core.ViewModel.Menu;
using foxer.Render.Menu;
using System.Drawing;

namespace foxer.Pages.Game.Menu
{
    public class InventoryMenu : MenuBase
    {
        public InventoryMenu(MenuInventoryViewModel viewModel) 
            : base(10, 6)
        {
            // M I K + + + + + + +   M I K + + +
            // C + + F + + 0 0 0 +   C + + + + +
            // + + + F + + 0 0 0 +   + + + + + +
            // + + + + + + 0 0 0 +   + F F F F +
            // + + + + + + 0 0 0 +   + + + + + +
            // + + + + + + + + + +   + + + + + +
            //                       + 0 0 0 0 +
            //                       + 0 0 0 0 +
            //                       + 0 0 0 0 +
            //                       + + + + + +

            BeginCreateCell(new MenuButton(viewModel.CommandCancel, Properties.Resources.icon_cancel)) // C
                .SetDefaultLayout(0, 1).End();

            BeginCreateCell(new MenuButton(viewModel.CommandOptions, Properties.Resources.icon_menu)) // M
                .SetDefaultLayout(0, 0).End();

            BeginCreateCell(new MenuButton(null, Properties.Resources.icon_inventory)) // I
                .SetDefaultLayout(1, 0).End();

            BeginCreateCell(new MenuButton(viewModel.CommandCraft, Properties.Resources.icon_craft)) // K
                .SetDefaultLayout(2, 0).End();

            for (int j = 0; j < viewModel.FastPanel.Length; j++)
            {
                BeginCreateCell(new MenuCell(viewModel.InventoryManager, viewModel.FastPanel[j])) // F
                    .SetDefaultLayout(3, j + 1).SetTransponedLayout(j + 1, 3).End();
            }

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
