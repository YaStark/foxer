using System.Drawing;

namespace foxer.Core.Game.Entities
{
    public interface IWall
    {
        Point Cell { get; }

        bool Active(Stage stage);

        Point GetTransitPreventionTarget();
    }
}
