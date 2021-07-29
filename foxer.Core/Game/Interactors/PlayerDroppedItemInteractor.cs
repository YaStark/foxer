using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using System.Drawing;

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

            Point[] path = null;
            if (MathUtils.L1(player.Cell, entity.Cell) > 1)
            {
                path = GetPathToItem(player, entity, arg.Stage);
                if (path == null)
                {
                    return false;
                }
            }

            player.MoveThenDo(
                path,
                GameUtils.DelegateCoroutine(x =>
                {
                    player.RotateTo(entity.Cell);
                    entity.DoGather(player, i => arg.Stage.InventoryManager.Accomodate(i));
                }));

            return true;
        }
    }
}
