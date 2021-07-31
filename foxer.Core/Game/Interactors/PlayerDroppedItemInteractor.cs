using foxer.Core.Game.Entities;
using foxer.Core.Utils;

namespace foxer.Core.Game.Interactors
{
    public class PlayerDroppedItemInteractor : InteractorBase<PlayerEntity>
    {
        protected override bool CanInteract(PlayerEntity subj, object obj, InteractorArgs arg)
        {
            return obj is DroppedItemEntity entity
                && arg.Stage.InventoryManager.CanAccomodate(entity.Item);
        }

        protected override bool Interact(PlayerEntity player, object obj, InteractorArgs arg)
        {
            if(!(obj is DroppedItemEntity entity))
            {
                return false;
            }

            return player.TryRunToTarget(
                arg.Stage, 
                entity,
                1,
                GameUtils.DelegateCoroutine(eca => entity.DoGather(player, i => eca.Stage.InventoryManager.Accomodate(i))));
        }
    }
}
