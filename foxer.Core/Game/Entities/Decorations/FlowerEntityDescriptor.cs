using System.Collections.Generic;
using foxer.Core.Game.Cells;

namespace foxer.Core.Game.Entities
{
    public class FlowerEntityDescriptor : EntityDescriptor<FlowerEntity>
    {
        public FlowerEntityDescriptor() 
            : base(EntityKind.SmallItem)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IEnumerable<EntityBase> entites, IPlatform platform)
        {
            return base.OnCanBePlaced(stage, cell, entites, platform)
                && platform == stage.DefaultPlatform
                && cell.Kind == CellKind.Floor;
        }
    }
}
