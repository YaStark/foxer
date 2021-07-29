using foxer.Core.Game.Items;
using foxer.Core.ViewModel.Menu;
using foxer.Render.Menu;
using foxer.Render.Menu.Items;
using System.Drawing;

namespace foxer.Pages.Game.Menu
{
    public abstract class InventoryMenuBase : MenuBase
    {
        public InventoryMenuBase(GameMenuViewModelBase viewModel, IInventoryManager inventoryManager, string title)
            : base(viewModel, 13, 8)
        {
            // M I K + [ TITLE ] + + + +   M C [TITLE] + +
            // C + + + + + + F F f f + +   I + + + + + + +
            // + + + + + + + 0 0 0 0 0 +   K + + + + + + +
            // + + + + + + + 0 0 0 0 0 +   + + + + + + + +
            // + + + + + + + 0 0 0 0 0 +   + + + + + + + +
            // + + + + + + + 0 0 0 0 0 +   + + + + + + + +
            // + + + + + + + V V V V V +   + + + + + + + +
            // + + + + + + + + + + + + +   + F 0 0 0 0 0 +
            //                             + F 0 0 0 0 0 +
            //                             + f 0 0 0 0 0 +
            //                             + f 0 0 0 0 0 +
            //                             + + V V V V V +
            //                             + + + + + + + +

            BeginCreateCell(new MenuText(title, Color.Black, true))
                .SetDefaultLayout(5, 0, 5, 1).SetTransponedLayout(3, 0, 4, 1).End(); // TITLE

            BeginCreateCell(new MenuButton(viewModel.CommandCancel, Properties.Resources.icon_cancel)) // C
                .SetDefaultLayout(0, 1).SetTransponedLayout(1, 0).End();

            BeginCreateCell(new MenuButton(viewModel.CommandOptions, Properties.Resources.icon_menu)) // M
                .SetDefaultLayout(0, 0).End();

            BeginCreateCell(new MenuButton(viewModel.CommandInventory, Properties.Resources.icon_inventory)) // I
                .SetDefaultLayout(1, 0).SetTransponedLayout(0, 1).End();

            BeginCreateCell(new MenuButton(viewModel.CommandCraft, Properties.Resources.icon_craft)) // K
                .SetDefaultLayout(2, 0).SetTransponedLayout(0, 2).End();

            BeginCreateCell(new MenuItemInfoPanel(viewModel.ItemInfoProviderFactory)) // V
                .SetDefaultLayout(7, 6, 5, 1).SetTransponedLayout(2, 11, 5, 1).End();

            for (int i = 0; i < viewModel.FastPanel.Length; i++)
            {
                BeginCreateCell(MenuCell(viewModel, inventoryManager, viewModel.FastPanel[i])) // F
                    .SetDefaultLayout(7 + i, 1).SetTransponedLayout(1, 7 + i).End();
            }

            for (int i = 0; i < viewModel.Inventory.GetLength(0); i++)
            {
                for (int j = 0; j < viewModel.Inventory.GetLength(1); j++)
                {
                    BeginCreateCell(MenuCell(viewModel, inventoryManager, viewModel.Inventory[i, j])) // 0
                        .SetDefaultLayout(7 + i, 2 + j).SetTransponedLayout(2 + i, 7 + j).End();
                }
            }
        }

        private IMenuItem MenuCell(GameMenuViewModelBase viewModel, IInventoryManager inventoryManager, IItemHolder itemHolder)
        {
            return new MenuCell(inventoryManager, viewModel.ItemManager, itemHolder);
        }

        public override bool Touch(PointF pt, SizeF size)
        {
            base.Touch(pt, size);
            return true;
        }
    }
}
