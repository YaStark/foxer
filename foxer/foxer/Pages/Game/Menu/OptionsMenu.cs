using foxer.Core.ViewModel.Menu;
using foxer.Render.Menu;
using System.Drawing;

namespace foxer.Pages.Game.Menu
{
    public class OptionsMenu : MenuBase
    {
        public OptionsMenu(MenuOptionsViewModel viewModel)
            : base(viewModel, 13, 8)
        {
            // C E + + [ MNU ] + + + +  C E [ MNU ] + +
            // + + + + + + + + + + + +  + + + + + + + +
            // + + + + + + + + + + Z +  + + + + + + + +
            // + + + + + + + + + + z +  + + + + + + Z +
            // + + + + + + + + + + + +  + + + + + + z +
            // + + + + + + + + + + + +  + + + + + + + +
            // + + + + + + + + + + + +  + + + + + + + +
            // + + + + + + + + + + + +  + + + + + + + +
            //                          + + + + + + + +
            //                          + + + + + + + +
            //                          + + + + + + + +
            //                          + + + + + + + +

            BeginCreateCell(new MenuButton(viewModel.CommandCancel, Properties.Resources.icon_cancel)) // C
                .SetDefaultLayout(0, 0).End();

            BeginCreateCell(new MenuButton(viewModel.CommandExit, Properties.Resources.icon_exit)) // E
                .SetDefaultLayout(1, 0).SetTransponedLayout(0, 1).End();

            BeginCreateCell(new MenuButton(viewModel.CommandZoomIn, Properties.Resources.icon_zoom_in)) // Z
                .SetDefaultLayout(10, 2).SetTransponedLayout(6, 3).End();

            BeginCreateCell(new MenuText("Menu", Color.Black, true))   // [ MNU ] 
                .SetDefaultLayout(4, 0, 4, 1).SetTransponedLayout(2, 0, 4, 1).End();

            // settings

            BeginCreateCell(new MenuButton(viewModel.CommandZoomOut, Properties.Resources.icon_zoom_out)) // z
                .SetDefaultLayout(10, 3).SetTransponedLayout(6, 4).End();

        }

        public override bool Touch(PointF pt, SizeF size)
        {
            base.Touch(pt, size);
            return true;
        }
    }
}