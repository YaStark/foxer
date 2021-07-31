using foxer.Core.Game.Entities;

namespace foxer.Core.Game
{
    public interface IWeaponItem
    {
        WeaponKind WeaponKind { get; }
        int SwipeMs { get; }
        int HitMs { get; }
        int Distance { get; }
        bool CanInteract(EntityFighterBase entity);
        int GetDamage(Stage stage, EntityBase target);
    }
}
