using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using System.Drawing;

namespace foxer.Core.Game.Interactors
{
    public class PlayerDroppedItemInteractor : InteractorBase<PlayerEntity, DroppedItemEntity>
    {
        protected override bool CanInteract(PlayerEntity subj, DroppedItemEntity obj, InteractorArgs arg)
        {
            return arg.Stage.InventoryManager.CanAccomodate(obj.Item);
        }

        protected override bool Interact(PlayerEntity player, DroppedItemEntity droppedItem, InteractorArgs arg)
        {
            Point[] path = null;
            if (MathUtils.L1(player.Cell, droppedItem.Cell) > 1)
            {
                path = GetPathToItem(player, droppedItem, arg.Stage);
                if (path == null)
                {
                    return false;
                }
            }

            player.MoveThenDo(
                path,
                GameUtils.DelegateCoroutine(x =>
                {
                    player.RotateTo(droppedItem.Cell);
                    droppedItem.DoGather(player, i => arg.Stage.InventoryManager.Accomodate(i));
                }));

            return true;
        }
    }
}
