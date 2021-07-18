namespace foxer.Core.Game.Items
{
    public class ItemStone : StackableItemBase
    {
        public ItemStone(int count)
            : base(ItemKind.Resource, 37)
        {
            Count = count;
        }
    }
}
