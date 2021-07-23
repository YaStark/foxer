using System.Collections.Generic;
using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities.Descriptors;

namespace foxer.Core.Game.Entities
{
    public class FlowerEntityDescriptor : EntityDescriptor<FlowerEntity>
    {
        public FlowerEntityDescriptor() 
            : base(EntityKind.SmallItem)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IEnumerable<EntityBase> entites, float z)
        {
            return base.OnCanBePlaced(stage, cell, entites, z)
                && cell.Kind == CellKind.Floor;
        }
    }
}
