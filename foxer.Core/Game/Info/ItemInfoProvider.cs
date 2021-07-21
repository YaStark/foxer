namespace foxer.Core.Game.Info
{
    public interface IItemInfoProvider
    {
        string GetName(object item, Stage stage);
        string GetDescription(object item, Stage stage);
    }

    public interface IItemInfoProviderWideTyped : IItemInfoProvider
    {
        bool Match<T>(T item);
    }
}
