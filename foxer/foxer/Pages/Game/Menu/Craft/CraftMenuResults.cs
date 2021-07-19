using System.Drawing;
using System.Linq;
using foxer.Core.Utils;
using foxer.Core.ViewModel.Menu.Craft;
using foxer.Render;
using foxer.Render.Items;
using foxer.Render.Menu;

namespace foxer.Pages.Game.Menu.Craft
{
    public class CraftMenuResults : MenuButton
    {
        private static readonly ItemRendererFactory _itemRendererFactory = new ItemRendererFactory();
        private readonly ICraftMenuManager _craftMenuManager;

        public CraftMenuResults(ICraftMenuManager craftMenuManager)
            : base(null, null)
        {
            _craftMenuManager = craftMenuManager;
        }

        protected override void OnRender(INativeCanvas canvas, MenuItemInfoArgs args)
        {
            Enabled = _craftMenuManager.Selected?.CanCraft(args.Stage) == true;
            base.OnRender(canvas, args);

            if (_craftMenuManager.Selected == null)
            {
                return;
            }

            var cs = args.CellSize;
            var bounds = GeomUtils.Deflate(args.Bounds, cs.Width * 0.2f, cs.Height * 0.2f);
            var result = _craftMenuManager.Selected.GetResult();
            if (result.Count == 1)
            {
                // draw in a middle
                var kv = result.First();
                _itemRendererFactory.GetRenderer(kv.Key)
                    ?.RenderForCraft(canvas, kv.Value.ToString(), GeomUtils.DeflateTo(bounds, args.CellSize), true);
                return;
            }

            int width = (int)(args.Bounds.Width / cs.Width);
            int height = (int)(args.Bounds.Height / cs.Height);
            var cells = GeomUtils.SplitOnCells(bounds, width, height);
            int k = 0;
            foreach (var kv in _craftMenuManager.Selected.GetResult())
            {
                var cell = cells[k % width, k / width];
                _itemRendererFactory.GetRenderer(kv.Key)
                    ?.RenderForCraft(canvas, kv.Value.ToString(), cell, true);

                k++;
                if (k > width * height)
                {
                    // todo ???
                    return;
                }
            }
        }

        protected override bool OnTouch(PointF pt, MenuItemInfoArgs args)
        {
            if(_craftMenuManager.Selected != null)
            {
                // todo craft errors?
                _craftMenuManager.Crafter.Craft(args.Stage, _craftMenuManager.Selected);
            }

            return base.OnTouch(pt, args);
        }
    }
}
