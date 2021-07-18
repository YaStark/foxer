namespace foxer.Core.Utils
{
    public interface ICoroutine
    {
        void Update();
        bool Finished { get; }
    }
}
