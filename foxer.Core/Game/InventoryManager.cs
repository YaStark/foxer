using foxer.Core.Game.Items;

namespace foxer.Core.Game
{
    public class InventoryManager
    {
        private Game _game;

        public InventoryManager(Game game)
        {
            _game = game;
        }

        public ItemBase GetFastPanel(int i)
        {
            if (i < 0 || i > _game.FastPanelSize) return null;
            return _game.Inventory1[i];
        }

        public void SetFastPanel(int i, ItemBase item)
        {
            if (i < 0 || i > _game.FastPanelSize) return;
            _game.Inventory1[i] = item;
        }

        public ItemBase GetInventory(int i, int j)
        {
            int k = Game.MAX_FAST_PANEL_SIZE + i + j * _game.InventorySize.Width;
            if (k < 0 || k >= _game.Inventory1.Length) return null;
            return _game.Inventory1[k];
        }

        public void SetInventory(int i, int j, ItemBase item)
        {
            int k = Game.MAX_FAST_PANEL_SIZE + i + j * _game.InventorySize.Width;
            if (k < 0 || k >= _game.Inventory1.Length) return;
            _game.Inventory1[k] = item;
        }

        public bool CanAccomodate(ItemBase item)
        {
            int count = item.Count;
            for (int i = 0; i < _game.Inventory1.Length; i++)
            {
                if(!CheckInventorySlotActive(i))
                {
                    continue;
                }

                if (CheckCanAccomodate(_game.Inventory1[i], item, ref count))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Contains<T>(int count)
        {
            for (int i = 0; i < _game.Inventory1.Length; i++)
            {
                if (!CheckInventorySlotActive(i)) continue;
                if(_game.Inventory1[i].GetType() == typeof(T))
                {
                    count -= _game.Inventory1[i].Count;
                    if (count <= 0) return true;
                }
            }

            return false;
        }

        public bool Accomodate(ItemBase item)
        {
            return StackWithExistedCells(item)
                || AddToEmptyCells(item);
        }

        public void Remove(ItemBase item)
        {
            for (int i = 0; i < _game.Inventory1.Length; i++)
            {
                if (!CheckInventorySlotActive(i)) continue;
                if(_game.Inventory1[i] == item)
                {
                    _game.Inventory1[i] = null;
                    return;
                }
            }
        }

        private bool StackWithExistedCells(ItemBase item)
        {
            if (!item.CanStack)
            {
                return false;
            }

            for (int i = 0; i < _game.Inventory1.Length; i++)
            {
                if (!CheckInventorySlotActive(i)) continue;
                if (TryStackWithExistedCell(_game.Inventory1[i], item))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckCanAccomodate(ItemBase cell, ItemBase item, ref int count)
        {
            if (cell == null)
            {
                return true;
            }
            else if (item.CanStack && cell.GetType() == item.GetType())
            {
                count -= cell.MaxStackSize - cell.Count;
            }

            return count <= 0;
        }

        private static bool TryStackWithExistedCell(ItemBase cell, ItemBase item)
        {
            if (cell?.GetType() != item.GetType())
            {
                return false;
            }

            int delta = cell.MaxStackSize - cell.Count;
            if (delta >= item.Count)
            {
                cell.Count += item.Count;
                item.Count = 0;
                return true;
            }

            cell.Count = cell.MaxStackSize;
            item.Count -= delta;
            return false;
        }

        private bool AddToEmptyCells(ItemBase item)
        {
            for (int i = 0; i < _game.Inventory1.Length; i++)
            {
                if (!CheckInventorySlotActive(i)) continue;
                if (_game.Inventory1[i] == null)
                {
                    _game.Inventory1[i] = item;
                    return true;
                }
            }

            return false;
        }
        
        private bool CheckInventorySlotActive(int i)
        {
            return i < _game.FastPanelSize || i >= Game.MAX_FAST_PANEL_SIZE;
        }
    }
}
