using foxer.Core.Game.Cells;
using System.Collections.Generic;

namespace foxer.Core.Game.Entities
{
    public class PlayerEntityDescriptor : EntityDescriptor<PlayerEntity>
    {
        public PlayerEntityDescriptor() 
            : base(EntityKind.BigCreature)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IEnumerable<EntityBase> entites, IPlatform platform)
        {
            if (!base.OnCanBePlaced(stage, cell, entites, platform))
            {
                return false;
            }

            if(platform == stage.DefaultPlatform)
            {
                return cell.Kind == CellKind.Door
                    || cell.Kind == CellKind.Floor
                    || cell.Kind == CellKind.Road;
            }

            return true;
        }
    }
}
