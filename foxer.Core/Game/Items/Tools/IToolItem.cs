using foxer.Core.Game.Entities;

namespace foxer.Core.Game.Items
{
    public interface IToolItem
    {
        WeaponKind WeaponKind { get; }
        int SwipeMs { get; }
        bool CanInteract(EntityBase entity);
        int GetSwipesCount(EntityBase entity);
    }

    public enum WeaponKind
    {
        Axe,
        Spear,
        Sword,
        Club,
        Magic
    }
}
