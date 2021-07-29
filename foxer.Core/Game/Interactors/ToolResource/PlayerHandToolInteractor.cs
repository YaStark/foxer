using foxer.Core.Game.Animation;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;
using foxer.Core.Utils;

namespace foxer.Core.Game.Interactors
{
    public class PlayerHandToolInteractor : PlayerResourceInteractorBase
    {
        protected override IToolItem GetTool(PlayerEntity player)
        {
            return player.EmptyHandTool;
        }

        protected override SimpleAnimation GetToolAnimation(PlayerEntity player)
        {
            return player.ShakeHands;
        }

        protected override void OnEndInteract(PlayerEntity player, EntityBase subj, InteractorArgs arg)
        {
            if(subj is GrassEntity grass)
            {
                grass.Cut();
                arg.Stage.CreateDroppedLootItem(subj);
                return;
            }

            base.OnEndInteract(player, subj, arg);
        }
    }
}
