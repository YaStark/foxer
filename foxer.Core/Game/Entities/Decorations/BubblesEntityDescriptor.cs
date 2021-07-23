using System.Collections.Generic;
using foxer.Core.Game.Cells;

namespace foxer.Core.Game.Entities
{
    public class BubblesEntityDescriptor : EntityDescriptor<BubblesEntity>
    {
        public BubblesEntityDescriptor() 
            : base(EntityKind.SmallItem)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IEnumerable<EntityBase> entites, float z)
        {
            return cell?.Kind == CellKind.Water;
        }
    }
}
