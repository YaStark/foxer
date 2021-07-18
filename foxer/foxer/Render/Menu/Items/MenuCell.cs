using foxer.Core.Game.Items;
using foxer.Core.ViewModel.Menu;
using foxer.Render.Items;
using foxer.Render.Menu.Animation;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Render.Menu
{
    public class MenuCell : MenuItemBase
    {
        private static readonly byte[] _imageNormal = Properties.Resources.cell_normal;
        private static readonly byte[] _imageHovered = Properties.Resources.cell_hovered;
        private static readonly byte[] _imageDisabled = Properties.Resources.cell_disabled;
        private static readonly byte[] _imageSelected = Properties.Resources.cell_selected;
        private static readonly byte[] _imageActive = Properties.Resources.cell_active;

        private static readonly ItemRendererFactory _itemRendererFactory = new ItemRendererFactory();
        private readonly IInventoryManager _inventoryManager;
        private readonly IItemHolder _itemHolder;

        public UIAnimation WaitAfterClick { get; }

        public ItemBase Item => _itemHolder.Get();

        public MenuCell(IInventoryManager inventoryManager, IItemHolder itemHolder)
        {
            _inventoryManager = inventoryManager;
            _itemHolder = itemHolder;
            WaitAfterClick = new UIWaitingAnimation(50);
        }
        
        protected override bool OnTouch(PointF pt, RectangleF bounds)
        {
            StartAnimation(WaitAfterClick.Coroutine, Select);
            return true;
        }

        private IEnumerable<UIAnimation> Select(UICoroutineArgs args)
        {
            _inventoryManager.SetSelected(_itemHolder);
            yield return null;
        }

        protected override void OnRender(INativeCanvas canvas, RectangleF bounds)
        {
            base.OnRender(canvas, bounds);
            canvas.DrawImage(GetImage(), bounds);
            if (_inventoryManager.GetSelected(_itemHolder))
            {
                canvas.DrawImage(_imageSelected, bounds);
            }

            var item = Item;
            if (item != null)
            {
                bounds.Inflate(-bounds.Width * 0.1f, -bounds.Height * 0.1f);
                _itemRendererFactory.GetRenderer(Item)?.Render(canvas, item, bounds, true);
            }
        }

        private byte[] GetImage()
        {
            if(ActiveAnimation == WaitAfterClick)
            {
                return _imageHovered;
            }

            if(_inventoryManager.GetActive(_itemHolder))
            {
                return _imageActive;
            }

            return _imageNormal;
        }
    }
}
