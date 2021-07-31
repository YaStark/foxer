using foxer.Core.Game.Animation;
using foxer.Core.Utils;

namespace foxer.Core.Game.Entities
{
    public interface IAttacker
    {
        SimpleAttackAnimation AttackAnimation { get; }

        IWeaponItem Weapon { get; }
    }

    public class SimpleAttacker : IAttacker
    {
        private readonly ISingletoneFactory<IWeaponItem> _weaponHost;

        public SimpleAttackAnimation AttackAnimation { get; }

        public IWeaponItem Weapon => _weaponHost.Item;

        public SimpleAttacker(SimpleAttackAnimation attackAnimation, ISingletoneFactory<IWeaponItem> weaponHost)
        {
            AttackAnimation = attackAnimation;
            _weaponHost = weaponHost;
        }
    }
}
