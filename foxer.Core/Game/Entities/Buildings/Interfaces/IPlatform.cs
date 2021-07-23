namespace foxer.Core.Game.Entities
{
    public interface IPlatform
    {
        float Level { get; }

        bool Active(Stage stage);
    }
}
