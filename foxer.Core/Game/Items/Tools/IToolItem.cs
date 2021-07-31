using foxer.Core.Game.Entities;

namespace foxer.Core.Game.Items
{
    public interface IToolItem
    {
        WeaponKind WeaponKind { get; }
        int SwipeMs { get; }
        int HitMs { get; }
        int Distance { get; }

        bool CanInteract(EntityBase entity);
        int GetSwipesCount(EntityBase entity);
    }
}
