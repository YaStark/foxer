using System.Drawing;

namespace foxer.Core.Game.Entities
{
    public interface IWall
    {
        int Rotation { get; }

        int CellX { get; }

        int CellY { get; }

        bool Active(Stage stage);

        bool CanTransit(int x, int y);
    }
}
