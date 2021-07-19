using foxer.Core.Game;
using foxer.Core.Game.Craft;
using foxer.Core.ViewModel.Menu.Craft;
using foxer.Render;
using foxer.Render.Items;
using foxer.Render.Menu;
using foxer.Render.Menu.Items;
using System.Drawing;
using System.Linq;

namespace foxer.Pages.Game.Menu.Craft
{
    public class CraftMenuListBoxItem : MenuListBoxItemBase
    {
        private static readonly ItemRendererFactory _itemRendererFactory = new ItemRendererFactory();
        private static readonly byte[] _selected = Properties.Resources.cell_selected;

        private readonly ItemCraftBase _itemCraft;
        private readonly ICraftMenuManager _craftMenuManager;

        public CraftMenuListBoxItem(ICraftMenuManager craftMenuManager, ItemCraftBase itemCraft)
        {
            _itemCraft = itemCraft;
            _craftMenuManager = craftMenuManager;
        }

        protected override void OnRender(INativeCanvas canvas, MenuItemInfoArgs args)
        {
            canvas.DrawImage(GetBackgroundImage(args.Stage), args.Bounds);

            var results = _itemCraft.GetResult();
            var first = results.First();
            _itemRendererFactory.GetRenderer(first.Key)
                ?.RenderForCraft(canvas, first.Value.ToString(), args.Bounds, first.Value > 1);

            if(GetSelected(args.Stage))
            {
                canvas.DrawImage(_selected, args.Bounds);
            }

            // todo render if the results is more than 1
        }

        protected override bool OnTouch(PointF pt, MenuItemInfoArgs args)
        {
            if(GetVisible(args.Stage))
            {
                _craftMenuManager.Selected = _itemCraft;
                return true;
            }

            return base.OnTouch(pt, args);
        }

        public override bool GetEnabled(Stage stage)
        {
            return _itemCraft.CanCraft(stage);
        }

        public override bool GetVisible(Stage stage)
        {
            return _craftMenuManager.Crafter.IsVisible(_itemCraft);
        }

        public override bool GetSelected(Stage stage)
        {
            return GetEnabled(stage) 
                && _craftMenuManager.Selected == _itemCraft;
        }
    }
}
