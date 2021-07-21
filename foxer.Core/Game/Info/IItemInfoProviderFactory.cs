namespace foxer.Core.Game.Info
{
    public interface IItemInfoProviderFactory
    {
        IItemInfoProvider GetItemInfoProvider();
        object GetItem();
    }
}
