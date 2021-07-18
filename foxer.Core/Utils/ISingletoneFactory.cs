namespace foxer.Core.Utils
{
    public interface ISingletoneFactory<T>
    {
        T Item { get; }
    }
}
