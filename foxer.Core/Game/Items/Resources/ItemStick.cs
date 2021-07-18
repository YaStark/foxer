namespace foxer.Core.Game.Items
{
    public class ItemStick : StackableItemBase
    {
        public ItemStick(int count)
            : base(ItemKind.Resource, 37)
        {
            Count = count;
        }
    }
}
