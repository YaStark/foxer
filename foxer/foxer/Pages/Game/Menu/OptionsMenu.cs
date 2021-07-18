using foxer.Core.ViewModel.Menu;
using foxer.Render.Menu;
using System.Drawing;

namespace foxer.Pages.Game.Menu
{
    public class OptionsMenu : MenuBase
    {
        public OptionsMenu(MenuOptionsViewModel vm)
            : base(10, 6)
        {
            // C E + [ MNU ] + + +   C E + + + +
            // + + + + + + + + + +   + [ MNU ] +
            // + + + + + + + + Z +   + + + + + +
            // + + + + + + + + z +   + + + + Z +
            // + + + + + + + + + +   + + + + z +
            // + + + + + + + + + +   + + + + + +
            //                       + + + + + +
            //                       + + + + + +
            //                       + + + + + +
            //                       + + + + + +

            BeginCreateCell(new MenuButton(vm.CommandCancel, Properties.Resources.icon_cancel)) // C
                .SetDefaultLayout(0, 0).End();

            BeginCreateCell(new MenuButton(vm.CommandExit, Properties.Resources.icon_exit)) // E
                .SetDefaultLayout(1, 0).End();

            BeginCreateCell(new MenuButton(vm.CommandZoomIn, Properties.Resources.icon_zoom_in)) // Z
                .SetDefaultLayout(8, 2).SetTransponedLayout(4, 3).End();

            BeginCreateCell(new MenuButton(vm.CommandZoomOut, Properties.Resources.icon_zoom_out)) // z
                .SetDefaultLayout(8, 3).SetTransponedLayout(4, 4).End();

            BeginCreateCell(new MenuText("Menu", Color.Black))   // [ MNU ] 
                .SetDefaultLayout(3, 0, 4, 1).SetTransponedLayout(1, 1, 4, 1).End();
        }

        public override bool Touch(PointF pt, SizeF size)
        {
            base.Touch(pt, size);
            return true;
        }
    }
}