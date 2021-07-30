using foxer.Core.Game.Animation;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;

namespace foxer.Core.Game.Interactors
{
    public class PlayerTreeAxeInteractor : PlayerResourceInteractorBase
    {
        protected override bool CanInteract(PlayerEntity player, object obj, InteractorArgs arg)
        {
            return base.CanInteract(player, obj, arg)
                && player.Hand is ItemStoneAxe;
        }

        protected override void OnToolSwipe(PlayerEntity player, EntityBase entity, InteractorArgs arg)
        {
            base.OnToolSwipe(player, entity, arg);
            if(entity is TreeEntity tree)
            {
                tree.DoShake();
            }
        }
    }
}
