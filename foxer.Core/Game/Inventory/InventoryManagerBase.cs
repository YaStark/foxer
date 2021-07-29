using foxer.Core.Game.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace foxer.Core.Game.Inventory
{
    public abstract class InventoryManagerBase
    {
        public ItemManager ItemManager { get; }
        public IList<ItemBase>[] Inventories { get; }

        protected InventoryManagerBase(ItemManager itemManager, params IList<ItemBase>[] inventories)
        {
            ItemManager = itemManager;
            Inventories = inventories;
        }

        public bool CanAccomodate(Type itemType, int count)
        {
            foreach (var inventory in Inventories)
            {
                for (int i = 0; i < inventory.Count; i++)
                {
                    if (CheckInventorySlotActive(inventory, i)
                        && CheckCanAccomodate(inventory[i], itemType, ref count))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool CanAccomodate<TItem>(int count)
        {
            return CanAccomodate(typeof(TItem), count);
        }

        public bool CanAccomodate(ItemBase item)
        {
            return CanAccomodate(item.GetType(), item.Count);
        }

        public int Count(Type itemType)
        {
            int count = 0;
            foreach (var inventory in Inventories)
            {
                for (int i = 0; i < inventory.Count; i++)
                {
                    if (CheckInventorySlotActive(inventory, i)
                        && inventory[i]?.GetType() == itemType)
                    {
                        count += inventory[i].Count;
                    }
                }
            }

            return count;
        }

        public bool TrySpend(Type[] itemTypes, int count)
        {
            foreach (var inventory in Inventories)
            {
                for (int i = 0; i < inventory.Count; i++)
                {
                    if (CheckInventorySlotActive(inventory, i)
                        && itemTypes.Contains(inventory[i]?.GetType()))
                    {
                        if(ItemManager.CanStack(inventory[i]))
                        {
                            var diff = Math.Min(inventory[i].Count, count);
                            inventory[i].Count -= diff;
                            if(inventory[i].Count == 0)
                            {
                                inventory[i] = null;
                            }

                            count -= diff;
                        }
                        else
                        {
                            inventory[i] = null;
                            count--;
                        }

                        if (count <= 0)
                        {
                            return true;
                        }
                    }
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
            foreach (var inventory in Inventories)
            {
                for (int i = 0; i < inventory.Count; i++)
                {
                    if (CheckInventorySlotActive(inventory, i)
                        && inventory[i] == item)
                    {
                        inventory[i] = null;
                        return;
                    }
                }
            }
        }

        private bool StackWithExistedCells(ItemBase item)
        {
            if (!ItemManager.CanStack(item))
            {
                return false;
            }

            foreach (var inventory in Inventories)
            {
                for (int i = 0; i < inventory.Count; i++)
                {
                    if (CheckInventorySlotActive(inventory, i)
                        && TryStackWithExistedCell(inventory[i], item))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckCanAccomodate(ItemBase cell, Type itemType, ref int count)
        {
            if (cell == null)
            {
                return true;
            }
            else if (ItemManager.CanStack(itemType) 
                && cell.GetType() == itemType)
            {
                count -= ItemManager.MaxStackSize(itemType) - cell.Count;
            }

            return count <= 0;
        }

        private bool TryStackWithExistedCell(ItemBase cell, ItemBase item)
        {
            if (cell?.GetType() != item.GetType())
            {
                return false;
            }

            int delta = ItemManager.MaxStackSize(cell) - cell.Count;
            if (delta >= item.Count)
            {
                cell.Count += item.Count;
                item.Count = 0;
                return true;
            }

            cell.Count = ItemManager.MaxStackSize(item);
            item.Count -= delta;
            return false;
        }

        private bool AddToEmptyCells(ItemBase item)
        {
            foreach (var inventory in Inventories)
            {
                for (int i = 0; i < inventory.Count; i++)
                {
                    if (CheckInventorySlotActive(inventory, i)
                        && inventory[i] == null)
                    {
                        inventory[i] = item;
                        return true;
                    }
                }
            }

            return false;
        }

        protected virtual bool CheckInventorySlotActive(IList<ItemBase> inventory, int i)
        {
            return true;
        }
    }
}
