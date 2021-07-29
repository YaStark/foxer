using foxer.Core.Game.Animation;
using foxer.Core.Game.Entities;

namespace foxer.Core.Game.Interactors
{
    public class DefaultToolInteractor : PlayerResourceInteractorBase
    {
        protected override bool CanInteract(PlayerEntity player, object obj, InteractorArgs arg)
        {
            return base.CanInteract(player, obj, arg);
        }

        protected override SimpleAnimation GetToolAnimation(PlayerEntity player)
        {
            return player.ShakeHands;
        }
    }
}
