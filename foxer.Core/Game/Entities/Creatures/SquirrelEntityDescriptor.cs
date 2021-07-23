using foxer.Core.Game.Entities.Descriptors;
using foxer.Core.Game.Entities.Stress;

namespace foxer.Core.Game.Entities
{
    public class SquirrelEntityDescriptor : EntityDescriptor<SquirrelEntity>
    {
        private static readonly StressInfo _wolfStress = new StressInfo(10, 2);

        public override IStresser Stresser { get; } = new Stresser<SquirrelEntity>()
                                                        .Add<PlayerEntity>(GetStressFromPlayer)
                                                        .Add<WolfEntity>(_wolfStress);

        public SquirrelEntityDescriptor() 
            : base(EntityKind.SmallCreature)
        {
        }

        protected override bool CheckCanOtherBePlacedHere(EntityDescriptorBase descriptor)
        {
            return descriptor.EntityType == typeof(TreeEntity)
                || base.CheckCanOtherBePlacedHere(descriptor);
        }

        private static StressInfo GetStressFromPlayer(PlayerEntity player)
        {
            return new StressInfo(player.AggressionLevel + 1, player.AggressionLevel / 2);
        }
    }
}
