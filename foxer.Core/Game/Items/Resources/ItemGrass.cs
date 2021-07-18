namespace foxer.Core.Game.Items
{
    public class ItemGrass : StackableItemBase
    {
        public ItemGrass(int count)
            : base(ItemKind.Resource, 37)
        {
            Count = count;
        }
    }
}
