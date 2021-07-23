using System.Collections.Generic;
using foxer.Core.Game.Cells;
using foxer.Core.Game.Items;

namespace foxer.Core.Game.Entities
{
    public class StoneSmallEntityDescriptor : EntityDescriptor<StoneSmallEntity>
    {
        public StoneSmallEntityDescriptor() 
            : base(EntityKind.SmallItem)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IEnumerable<EntityBase> entites, float z)
        {
            return base.OnCanBePlaced(stage, cell, entites, z)
                && cell.Kind == CellKind.Floor;
        }

        protected override ItemBase OnGetLoot(Stage stage, StoneSmallEntity entity)
        {
            return GetRndLoot<ItemStone>(stage, 1, 4);
        }
    }
}
