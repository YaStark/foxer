using foxer.Core.Game.Entities;

namespace foxer.Core.Game.Items
{
    public interface IBuildableWall : IBuildableItem
    {
        WallKind WallKind { get; set; }
    }
}
