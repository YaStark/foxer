using foxer.Core.Game.Entities;

namespace foxer.Core.Game.Items
{
    public interface IToolItem
    {
        int SwipeMs { get; }
        bool CanInteract(EntityBase entity);
        int GetSwipesCount(EntityBase entity);
    }
}
