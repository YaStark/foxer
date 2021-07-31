using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;

namespace foxer.Core.Game.Interactors
{
    public class PlayerAttackInteractor : InteractorBase<PlayerEntity>
    {
        private IWeaponItem GetTool(PlayerEntity player)
        {
            return player.Hand as IWeaponItem;
        }

        protected override bool CanInteract(PlayerEntity player, object obj, InteractorArgs arg)
        {
            return obj is EntityBase entity
                && GetTool(player)?.CanInteract(entity) == true;
        }

        protected override bool Interact(PlayerEntity player, object obj, InteractorArgs arg)
        {
            return obj is EntityBase entity
                && GetTool(player)?.CanInteract(entity) == true
                && player.TryAttack(arg.Stage, entity);
        }
    }
}
