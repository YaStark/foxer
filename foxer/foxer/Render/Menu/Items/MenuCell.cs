using foxer.Core.Game.Items;
using foxer.Core.Utils;
using foxer.Core.ViewModel.Menu;
using foxer.Render.Items;
using foxer.Render.Menu.Animation;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Render.Menu
{
    public class MenuCell : MenuItemBase
    {
        private static readonly MenuCellRenderer _renderer = new MenuCellRenderer();

        private static readonly ItemRendererFactory _itemRendererFactory = new ItemRendererFactory();
        private readonly ItemManager _itemManager;
        private readonly IInventoryManager _inventoryManager;
        private readonly IItemHolder _itemHolder;

        public ItemBase Item => _itemHolder.Get();

        public MenuCell(IInventoryManager inventoryManager, ItemManager itemManager, IItemHolder itemHolder)
        {
            _itemManager = itemManager;
            _inventoryManager = inventoryManager;
            _itemHolder = itemHolder;
        }
        
        protected override bool OnTouch(PointF pt, MenuItemInfoArgs args)
        {
            StartAnimation(_renderer.WaitAfterClick.Coroutine, Select);
            return true;
        }

        private IEnumerable<UIAnimation> Select(UICoroutineArgs args)
        {
            _inventoryManager.SetSelected(_itemHolder);
            yield return null;
        }

        protected override void OnRender(INativeCanvas canvas, MenuItemInfoArgs args)
        {
            base.OnRender(canvas, args);

            _renderer.Render(
                canvas,
                args,
                ActiveAnimation == _renderer.WaitAfterClick,
                _inventoryManager.GetActive(_itemHolder),
                _inventoryManager.GetSelected(_itemHolder));

            var item = Item;
            if (item != null)
            {
                var bounds = GeomUtils.Deflate(args.Bounds, args.Bounds.Width * 0.1f, args.Bounds.Height * 0.1f);
                bool showCount = _itemManager.CanStack(item);
                _itemRendererFactory.GetRenderer(Item)
                    ?.Render(canvas, item, bounds, showCount);
            }
        }
    }
}
