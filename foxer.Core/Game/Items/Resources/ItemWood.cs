namespace foxer.Core.Game.Items
{
    public class ItemWood : StackableItemBase
    {
        public ItemWood(int count)
            : base(ItemKind.Resource, 37)
        {
            Count = count;
        }
    }
}
