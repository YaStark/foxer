using foxer.Core.Game.Cells;
using System.Collections.Generic;

namespace foxer.Core.Game.Entities
{
    public class WolfEntityDescriptor : EntityDescriptor<WolfEntity>
    {
        public WolfEntityDescriptor() 
            : base(EntityKind.BigCreature)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IEnumerable<EntityBase> entites, float z)
        {
            if (!base.OnCanBePlaced(stage, cell, entites, z)) return false;
            return cell.Kind == CellKind.Floor
                || cell.Kind == CellKind.Road;
        }
    }
}
