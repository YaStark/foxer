using System.Collections.Generic;
using foxer.Core.Game.Cells;
using foxer.Core.Game.Items;

namespace foxer.Core.Game.Entities
{
    public class StoneBigEntityDescriptor : EntityDescriptor<StoneBigEntity>
    {
        public StoneBigEntityDescriptor()
            : base(EntityKind.BigItem)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IPlatform platform)
        {
            return platform == stage.DefaultPlatform
                && cell.Kind == CellKind.Floor;
        }

        protected override ItemBase OnGetLoot(Stage stage, StoneBigEntity entity)
        {
            return GetRndLoot<ItemStone>(stage, 5, 8);
        }
    }
}
