using System.Collections.Generic;
using foxer.Core.Game.Inventory;
using foxer.Core.Game.Items;

namespace foxer.Core.Game
{
    public class InventoryManager : InventoryManagerBase
    {
        private Game _game;

        public InventoryManager(Game game)
            : base(game.ItemManager, game.Inventory)
        {
            _game = game;
        }

        public ItemBase GetFastPanel(int i)
        {
            if (i < 0 || i > _game.FastPanelSize) return null;
            return _game.Inventory[i];
        }

        public void SetFastPanel(int i, ItemBase item)
        {
            if (i < 0 || i > _game.FastPanelSize) return;
            _game.Inventory[i] = item;
        }

        public ItemBase GetInventory(int i, int j)
        {
            int k = Game.MAX_FAST_PANEL_SIZE + i + j * _game.InventorySize.Width;
            if (k < 0 || k >= _game.Inventory.Length) return null;
            return _game.Inventory[k];
        }

        public void SetInventory(int i, int j, ItemBase item)
        {
            int k = Game.MAX_FAST_PANEL_SIZE + i + j * _game.InventorySize.Width;
            if (k < 0 || k >= _game.Inventory.Length) return;
            _game.Inventory[k] = item;
        }

        protected override bool CheckInventorySlotActive(IList<ItemBase> inventory, int i)
        {
            if(inventory == _game.Inventory)
            {
                return i < _game.FastPanelSize || i >= Game.MAX_FAST_PANEL_SIZE;
            }

            return true;
        }
    }
}
