using System.Collections.Generic;
using foxer.Core.Game.Cells;

namespace foxer.Core.Game.Entities
{
    public class StoneOvenEntityDescriptor : EntityDescriptor<StoneOvenEntity>
    {
        public StoneOvenEntityDescriptor() 
            : base(EntityKind.BigItem)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IEnumerable<EntityBase> entites, float z)
        {
            return base.OnCanBePlaced(stage, cell, entites, z)
                && cell.Kind == CellKind.Floor;
        }
    }
}
