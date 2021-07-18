using foxer.Core.Game.Animation;
using foxer.Core.Game.Entities;

namespace foxer.Core.Game.Interactors
{
    public class PlayerTreeAxeInteractor : PlayerResourceInteractorBase<TreeEntity>
    {
        protected override SimpleAnimation GetToolAnimation(PlayerEntity player)
        {
            return player.Chop;
        }

        protected override void OnToolSwipe(PlayerEntity player, TreeEntity tree, InteractorArgs arg)
        {
            base.OnToolSwipe(player, tree, arg);
            tree.DoShake();
        }
    }
}
