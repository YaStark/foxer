using System.Collections.Generic;
using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities.Descriptors;

namespace foxer.Core.Game.Entities
{
    public abstract class PlatformEntityDescriptorBase<TEntity> : EntityDescriptor<TEntity>
        where TEntity : PlatformEntityBase
    {
        protected PlatformEntityDescriptorBase() 
            : base(EntityKind.BigItem)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IEnumerable<EntityBase> entites, IPlatform platform)
        {
            if(!base.OnCanBePlaced(stage, cell, entites, platform))
            {
                return false;
            }

            if (platform == stage.DefaultPlatform)
            {
                return cell.Kind == CellKind.Floor;
            }

            return true;
        }

        protected override bool CheckCanOtherBePlacedHere(EntityDescriptorBase descriptor)
        {
            // any item can be placed here by default
            return true;
        }
    }
}
