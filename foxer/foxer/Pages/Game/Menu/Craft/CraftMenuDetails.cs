using System.Drawing;
using foxer.Core.Game.Craft;
using foxer.Core.Utils;
using foxer.Core.ViewModel.Menu.Craft;
using foxer.Render;
using foxer.Render.Items;
using foxer.Render.Menu;

namespace foxer.Pages.Game.Menu.Craft
{
    public class CraftMenuDetails : MenuItemBase
    {
        private static readonly byte[] _fail = Properties.Resources.icon_sup_fail;
        private static readonly byte[] _ok= Properties.Resources.icon_sup_ok;
        private static readonly ItemRendererFactory _itemRendererFactory = new ItemRendererFactory();
        private readonly ICraftMenuManager _craftMenuManager;

        public CraftMenuDetails(ICraftMenuManager craftMenuManager)
        {
            _craftMenuManager = craftMenuManager;
        }

        protected override void OnRender(INativeCanvas canvas, MenuItemInfoArgs args)
        {
            base.OnRender(canvas, args);

            RenderBackground(canvas, args.Bounds, args.CellSize);

            if (_craftMenuManager.Selected == null)
            {
                return;
            }

            var cs = args.CellSize;
            var bounds = GeomUtils.Deflate(args.Bounds, cs.Width * 0.2f, cs.Height * 0.2f);
            int width = (int)(args.Bounds.Width / cs.Width);
            int height = (int)(args.Bounds.Height / cs.Height);
            var cells = GeomUtils.SplitOnCells(bounds, width, height);
            int k = 0;
            foreach (var req in _craftMenuManager.Selected.GetRequirements())
            {
                var cell = cells[k % width, k / width];
                if(req is CraftResourceRequirementsBase resReq)
                {
                    _itemRendererFactory.GetRenderer(resReq.ItemType)
                        ?.RenderForCraft(canvas, resReq.Count.ToString(), cell, true);

                    if(!resReq.Match(args.Stage))
                    {
                        canvas.DrawImage(_fail, cell);
                    }
                    else
                    {
                        canvas.DrawImage(_ok, cell);
                    }
                }

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
            // todo scroll listbox?
            return base.OnTouch(pt, args);
        }
    }
}
