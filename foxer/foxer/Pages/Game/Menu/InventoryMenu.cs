using foxer.Core.Game.Items;
using foxer.Core.ViewModel.Menu;
using foxer.Render.Menu;
using System;
using System.Drawing;

namespace foxer.Pages.Game.Menu
{
    public class InventoryMenu : MenuBase
    {
        private readonly MenuInventoryViewModel _viewModel;

        public InventoryMenu(MenuInventoryViewModel viewModel) 
            : base(viewModel, 12, 8)
        {
            // M I K + + + + + + + + +   M I K + + + + +
            // C + + + + + + F F f f +   C + + + + + + +
            // + + + + + + + 0 0 0 0 +   + + + + + + + +
            // + + + + + + + 0 0 0 0 +   + + + + + + + +
            // + + + + + + + 0 0 0 0 +   + + + + + + + +
            // + + + + + + + 0 0 0 0 +   + + + + + + + +
            // + + + + + + + 0 0 0 0 +   + + + + + + + +
            // + + + + + + + + + + + +   + F 0 0 0 0 0 +
            //                           + F 0 0 0 0 0 +
            //                           + f 0 0 0 0 0 +
            //                           + f 0 0 0 0 0 +
            //                           + + + + + + + +

            _viewModel = viewModel;

            BeginCreateCell(new MenuButton(viewModel.CommandCancel, Properties.Resources.icon_cancel)) // C
                .SetDefaultLayout(0, 1).End();

            BeginCreateCell(new MenuButton(viewModel.CommandOptions, Properties.Resources.icon_menu)) // M
                .SetDefaultLayout(0, 0).End();

            BeginCreateCell(new MenuButton(null, Properties.Resources.icon_inventory)) // I
                .SetDefaultLayout(1, 0).End();

            BeginCreateCell(new MenuButton(viewModel.CommandCraft, Properties.Resources.icon_craft)) // K
                .SetDefaultLayout(2, 0).End();

            for (int i = 0; i < viewModel.FastPanel.Length; i++)
            {
                BeginCreateCell(MenuCell(viewModel, viewModel.FastPanel[i])) // F
                    .SetDefaultLayout(i + 7, 1).SetTransponedLayout(1, i + 7).End();
            }

            for (int i = 0; i < viewModel.Inventory.GetLength(0); i++)
            {
                for (int j = 0; j < viewModel.Inventory.GetLength(1); j++)
                {
                    BeginCreateCell(MenuCell(viewModel, viewModel.Inventory[i, j])) // 0
                        .SetDefaultLayout(7 + i, 2 + j).SetTransponedLayout(2 + j, 7 + i).End();
                }
            }
        }

        private IMenuItem MenuCell(MenuInventoryViewModel viewModel, IItemHolder itemHolder)
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
