using System.Collections.Generic;
using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities.Descriptors;
using foxer.Core.Game.Items;

namespace foxer.Core.Game.Entities
{
    public class StoneOvenEntityDescriptor : EntityDescriptor<StoneOvenEntity>
    {
        public StoneOvenEntityDescriptor() 
            : base(EntityKind.BigItem)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IEnumerable<EntityBase> entites, IPlatform platform)
        {
            if (!base.OnCanBePlaced(stage, cell, entites, platform))
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
            return descriptor.Kind == EntityKind.BigCreature
                || descriptor.Kind == EntityKind.SmallCreature;
        }

        protected override ItemBase OnGetLoot(Stage stage, StoneOvenEntity entity)
        {
            return stage.ItemManager.Create<ItemStoneOven>(stage, 1);
        }
    }
}
