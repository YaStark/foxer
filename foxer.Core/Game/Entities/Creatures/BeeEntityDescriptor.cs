using System.Collections.Generic;
using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities.Descriptors;
using foxer.Core.Game.Entities.Stress;

namespace foxer.Core.Game.Entities
{
    public class BeeEntityDescriptor : EntityDescriptor<BeeEntity>
    {
        private static StressInfo GetStressFromPlayer(PlayerEntity player)
        {
            return new StressInfo(player.AggressionLevel / 2, player.AggressionLevel / 4);
        }

        public override IStresser Stresser { get; } = new Stresser<SquirrelEntity>()
                                                        .Add<PlayerEntity>(GetStressFromPlayer);

        public BeeEntityDescriptor()
            : base(EntityKind.SmallCreature)
        {
        }

        protected override bool CheckCanOtherBePlacedHere(EntityDescriptorBase descriptor)
        {
            return true;
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IEnumerable<EntityBase> entites, float z)
        {
            return true;
        }
    }
}
