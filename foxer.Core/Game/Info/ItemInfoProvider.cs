namespace foxer.Core.Game.Info
{
    public interface IItemInfoProvider
    {
        string GetText(object item, Stage stage);
    }

    public interface IItemInfoProviderWideTyped : IItemInfoProvider
    {
        bool Match<T>(T item);
    }
}
