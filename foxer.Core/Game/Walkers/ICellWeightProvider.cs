using foxer.Core.Game.Entities;

namespace foxer.Core.Game
{
    public interface ICellWeightProvider
    {
        int GetCellWeight(Stage stage, EntityBase entity, int x, int y, IPlatform platform);
    }

    public interface ICellAccessibleProvider
    {
        bool CanWalk(Stage stage, EntityBase entity, int x, int y, IPlatform platform);
    }
}
