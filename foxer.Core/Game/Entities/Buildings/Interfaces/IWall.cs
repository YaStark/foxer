using System.Drawing;

namespace foxer.Core.Game.Entities
{
    public interface IWall
    {
        int Rotation { get; }

        Point Cell { get; }

        bool Active(Stage stage);

        bool CanTransit(Point to);
    }
}
