using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;

namespace foxer.Core.Game
{
    public interface IWeaponItem : IToolItem
    {
        int GetDamage(Stage stage, EntityBase target);
    }
}
