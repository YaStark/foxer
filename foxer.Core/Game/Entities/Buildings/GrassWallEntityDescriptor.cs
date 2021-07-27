using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities.Descriptors;
using System.Collections.Generic;

namespace foxer.Core.Game.Entities
{
    public class GrassWallEntityDescriptor : EntityDescriptor<GrassWallEntity>
    {
        public GrassWallEntityDescriptor()
            : base(EntityKind.BigItem)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IEnumerable<EntityBase> entites, IPlatform platform)
        {
            if(!base.OnCanBePlaced(stage, cell, entites, platform))
            {
                return false;
            }

            if(platform == stage.DefaultPlatform)
            {
                return cell.Kind == CellKind.Floor;
            }

            return true;
        }

        protected override bool CheckCanOtherBePlacedHere(EntityDescriptorBase descriptor)
        {
            return true;
        }
    }
}
