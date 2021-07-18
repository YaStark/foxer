using foxer.Core.Utils;
using foxer.Core.ViewModel.Menu;
using foxer.Render;
using foxer.Render.Menu;

namespace foxer.Pages.Game.Menu
{
    public class GameUI : MenuBase
    {
        private readonly GameUIViewModel _viewModel;
        private readonly ISingletoneFactory<IRenderer> _rendererFactory;

        public GameUI(GameUIViewModel viewModel, ISingletoneFactory<IRenderer> rendererFactory) 
            : base(10, 6)
        {
            // M I K + + + + + + F   M I K + + +
            // + + + + + + + + + F   + + + + + +
            // + + + + + + + + + F   + + + + + +
            // + + + + + + + + + F   + + + + + +
            // + + + + + + + + + +   + + + + + +
            // + + + + + + + + + +   + + + + + +
            //                       + + + + + +
            //                       + + + + + +
            //                       + + + + + +
            //                       + + F F F F

            _viewModel = viewModel;
            _rendererFactory = rendererFactory;

            BeginCreateCell(new MenuButton(viewModel.CommandOptions, Properties.Resources.icon_menu)) // M
                .SetDefaultLayout(0, 0).End();

            BeginCreateCell(new MenuButton(viewModel.CommandInventory, Properties.Resources.icon_inventory)) // I
                .SetDefaultLayout(1, 0).End();

            BeginCreateCell(new MenuButton(viewModel.CommandCraft, Properties.Resources.icon_craft)) // K
                .SetDefaultLayout(2, 0).End();

            for (int i = 0; i < viewModel.FastPanel.Length; i++)
            {
                BeginCreateCell(new MenuCell(viewModel.InventoryManager, viewModel.FastPanel[i])) // F
                    .SetDefaultLayout(9, i).SetTransponedLayout(5 - i, 9).End();
            }
        }
    }
}
