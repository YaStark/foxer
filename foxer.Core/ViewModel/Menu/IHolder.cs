using foxer.Core.Game.Craft;

namespace foxer.Core.Game.Items
{
    public interface IItemHolder : IHolder<ItemBase>
    {
    }

    public interface ICraftHolder : IHolder<ItemCraftBase>
    {
    }

    public interface IHolder<TType>
    {
        TType Get();
        void Set(TType item);
        void Clear();
    }
}
