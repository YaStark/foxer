using foxer.Core.Game.Items;
using foxer.Core.ViewModel.Menu;
using foxer.Render.Menu;

namespace foxer.Pages.Game.Menu
{
    public class GameUI : MenuBase
    {
        public GameUI(GameUIViewModel viewModel) 
            : base(viewModel, 13, 8)
        {
            // M I K + + + + + + + + W   M + + + + + + +
            // + + + + + + + + + + + F   I + + + + + + +
            // + + + + + + + + + + + F   K + + + + + + +
            // + + + + + + + + + + + F   + + + + + + + +
            // R R R + + + + + + + + F   + + + + + + + +
            // R R R + + + + + + + + +   + + + + + + + +
            // R R R + + + + + + + + +   + + + + + + + +
            // + + + B B B B + + + + +   + + + + + + + +
            //                           + + + + + R R R
            //                           + + + + + R R R
            //                           + + + + + R R R
            //                           W F F F F + + +

            BeginCreateCell(new MenuButton(viewModel.CommandOptions, Properties.Resources.icon_menu)) // M
                .SetDefaultLayout(0, 0).End();

            BeginCreateCell(new MenuButton(viewModel.CommandInventory, Properties.Resources.icon_inventory)) // I
                .SetDefaultLayout(1, 0).SetTransponedLayout(0, 1).End();

            BeginCreateCell(new MenuButton(viewModel.CommandCraft, Properties.Resources.icon_craft)) // K
                .SetDefaultLayout(2, 0).SetTransponedLayout(0, 2).End();

            BeginCreateCell(new WalkMenuItem(viewModel))
                .SetDefaultLayout(12, 0).SetTransponedLayout(0, 12).End(); // W

            BeginCreateCell(new EntityRotationMenuItem(viewModel)) // R
                .SetDefaultLayout(0, 4, 3, 3).SetTransponedLayout(5, 8, 3, 3).End();

            BeginCreateCell(new BuildingOptionsMenuItem(viewModel, 4))
                .SetDefaultLayout(3, 7, 4, 1).SetTransponedLayout(7, 3, 1, 4).End();

            for (int i = 0; i < viewModel.FastPanel.Length; i++)
            {
                BeginCreateCell(MenuCell(viewModel, viewModel.FastPanel[i])) // F
                    .SetDefaultLayout(12, 1 + i).SetTransponedLayout(1 + i, 12).End();
            }
        }

        private IMenuItem MenuCell(GameUIViewModel viewModel, IItemHolder itemHolder)
        {
            return new MenuCell(viewModel.InventoryManager, viewModel.ItemManager, itemHolder);
        }
    }
}
