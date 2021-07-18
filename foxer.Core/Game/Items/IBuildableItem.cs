using foxer.Core.Game.Entities;
using System.Drawing;

namespace foxer.Core.Game.Items
{
    public interface IBuildableItem
    {
        EntityBase Create(Stage stage, int x, int y);
        bool CheckCanBuild(Stage stage, int x, int y);
        bool CheckBuildDistance(Point player, Point target);
    }
}
