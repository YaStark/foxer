using foxer.Core.Game.Animation;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;
using foxer.Core.Utils;

namespace foxer.Core.Game.Interactors
{
    public class PlayerGrassInteractor : PlayerResourceInteractorBase<GrassEntity>
    {
        protected override IToolItem GetTool(PlayerEntity player)
        {
            return player.EmptyHandTool;
        }

        protected override SimpleAnimation GetToolAnimation(PlayerEntity player)
        {
            return player.ShakeHands;
        }
        
        protected override void OnEndInteract(PlayerEntity player, GrassEntity subj, InteractorArgs arg)
        {
            subj.Cut();
            arg.Stage.CreateDroppedLootItem(subj);
        }
    }
}
