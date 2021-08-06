using foxer.Core.Game.Cells;
using System.Collections.Generic;

namespace foxer.Core.Game.Entities
{
    public class CowEntityDescriptor : EntityDescriptor<CowEntity>
    {
        public CowEntityDescriptor()
            : base(EntityKind.BigCreature)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IPlatform platform)
        {
            if (platform == stage.DefaultPlatform)
            {
                return cell.Kind == CellKind.Floor
                    || cell.Kind == CellKind.Road;
            }

            return true;
        }
    }
}
