namespace foxer.Core.Game.Items
{
    public interface IItemHolder
    {
        ItemBase Get();
        void Set(ItemBase item);
        void Clear();
    }
}
