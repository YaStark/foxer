using foxer.Core.Game.Animation;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;
using System;

namespace foxer.Core.Game.Interactors
{
    public class SimpleToolResourceInteractor<TResource> : PlayerResourceInteractorBase<TResource>
        where TResource : EntityBase
    {
        private readonly Func<PlayerEntity, IToolItem> _toolFactory;
        private readonly Func<PlayerEntity, SimpleAnimation> _toolAnimationFactory;

        public SimpleToolResourceInteractor(
            Func<PlayerEntity, IToolItem> toolFactory, 
            Func<PlayerEntity, SimpleAnimation> toolAnimationFactory)
        {
            _toolFactory = toolFactory;
            _toolAnimationFactory = toolAnimationFactory;
        }

        protected override IToolItem GetTool(PlayerEntity player)
        {
            return _toolFactory.Invoke(player);
        }

        protected override SimpleAnimation GetToolAnimation(PlayerEntity player)
        {
            return _toolAnimationFactory.Invoke(player);
        }
    }
}
