using foxer.Core.Game.Animation;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;
using System;

namespace foxer.Core.Game.Interactors
{
    public static class SimpleToolResourceInteractors
    {
        public static IInteractor TreeHand { get; } = Def<TreeEntity>(p => p.EmptyHandTool, p => p.ShakeHands);

        public static IInteractor StoneHand { get; } = Def<StoneSmallEntity>(p => p.EmptyHandTool, p => p.ShakeHands);

        private static IInteractor Def<TEntity>(
            Func<PlayerEntity, IToolItem> toolFactory,
            Func<PlayerEntity, SimpleAnimation> toolAnimationFactory)
            where TEntity : EntityBase
        {
            return new SimpleToolResourceInteractor<TEntity>(toolFactory, toolAnimationFactory);
        }
    }
}
