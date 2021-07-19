﻿using foxer.Core.Game.Items;
using foxer.Core.Utils;
using foxer.Core.ViewModel.Menu;
using foxer.Render;
using foxer.Render.Menu;

namespace foxer.Pages.Game.Menu
{
    public class GameUI : MenuBase
    {
        public GameUI(GameUIViewModel viewModel) 
            : base(viewModel, 12, 8)
        {
            // M I K + + + + + + + + F   M I K + + + + +
            // + + + + + + + + + + + F   + + + + + + + +
            // + + + + + + + + + + + F   + + + + + + + +
            // + + + + + + + + + + + F   + + + + + + + +
            // + + + + + + + + + + + +   + + + + + + + +
            // + + + + + + + + + + + +   + + + + + + + +
            // + + + + + + + + + + + +   + + + + + + + +
            // + + + + + + + + + + + +   + + + + + + + +
            //                           + + + + + + + +
            //                           + + + + + + + +
            //                           + + + + + + + +
            //                           F F F F + + + +

            BeginCreateCell(new MenuButton(viewModel.CommandOptions, Properties.Resources.icon_menu)) // M
                .SetDefaultLayout(0, 0).End();

            BeginCreateCell(new MenuButton(viewModel.CommandInventory, Properties.Resources.icon_inventory)) // I
                .SetDefaultLayout(1, 0).End();

            BeginCreateCell(new MenuButton(viewModel.CommandCraft, Properties.Resources.icon_craft)) // K
                .SetDefaultLayout(2, 0).End();

            for (int i = 0; i < viewModel.FastPanel.Length; i++)
            {
                BeginCreateCell(MenuCell(viewModel, viewModel.FastPanel[i])) // F
                    .SetDefaultLayout(11, i).SetTransponedLayout(i, 11).End();
            }
        }

        private IMenuItem MenuCell(GameUIViewModel viewModel, IItemHolder itemHolder)
        {
            return new MenuCell(viewModel.InventoryManager, viewModel.ItemManager, itemHolder);
        }
    }
}
