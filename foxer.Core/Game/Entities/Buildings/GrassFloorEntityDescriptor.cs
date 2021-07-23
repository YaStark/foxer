using System.Collections.Generic;
using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities.Descriptors;

namespace foxer.Core.Game.Entities
{
    public class GrassFloorEntityDescriptor : EntityDescriptor<GrassFloorEntity>
    {
        public GrassFloorEntityDescriptor()
            : base(EntityKind.BigItem)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IEnumerable<EntityBase> entites, float z)
        {
            return base.OnCanBePlaced(stage, cell, entites, z)
                && cell.Kind == CellKind.Floor;
        }

        protected override bool CheckCanOtherBePlacedHere(EntityDescriptorBase descriptor)
        {
            // todo other walls?
            // todo зависит от текущей сущности - если напр. постройка слишком высока
            return true;
        }
    }
}
