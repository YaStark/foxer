using System.Drawing;
using System.Linq;
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

            if (_craftMenuManager.SelectedCraft == null)
            {
                return;
            }

            var cs = args.CellSize;
            var bounds = GeomUtils.Deflate(args.Bounds, cs.Width * 0.2f, cs.Height * 0.2f);
            int width = (int)(args.Bounds.Width / cs.Width);
            int height = (int)(args.Bounds.Height / cs.Height);
            var cells = GeomUtils.SplitOnCells(bounds, width, height);
            int k = 0;
            foreach (var req in _craftMenuManager.SelectedCraft.GetRequirements())
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
            if (_craftMenuManager.SelectedCraft == null)
            {
                return true;
            }

            var cs = args.CellSize;
            var bounds = GeomUtils.Deflate(args.Bounds, cs.Width * 0.2f, cs.Height * 0.2f);
            int width = (int)(args.Bounds.Width / cs.Width);
            int height = (int)(args.Bounds.Height / cs.Height);

            float x = pt.X - args.Bounds.Left;
            float y = pt.Y - args.Bounds.Top;
            int k = (int)(x * width / args.Bounds.Width) + (int)(y * height / args.Bounds.Height) * width;

            var requirements = _craftMenuManager.SelectedCraft.GetRequirements().ToArray();
            if (k >= 0 && k < requirements.Length)
            {
                _craftMenuManager.SetSelectedRequirement(requirements[k]);
            }
             else
            {
                _craftMenuManager.SetSelectedRequirement(null);
            }

            return true;
        }
    }
}
