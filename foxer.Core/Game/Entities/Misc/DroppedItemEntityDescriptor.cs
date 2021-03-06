using System.Collections.Generic;
using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities.Descriptors;

namespace foxer.Core.Game.Entities
{
    public class DroppedItemEntityDescriptor : EntityDescriptor<DroppedItemEntity>
    {
        public DroppedItemEntityDescriptor() 
            : base(EntityKind.SmallCreature)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IPlatform platform)
        {
            return true;
        }

        public override bool CheckCanOtherBePlacedHere(EntityDescriptorBase descriptor)
        {
            return true;
        }
    }
}
