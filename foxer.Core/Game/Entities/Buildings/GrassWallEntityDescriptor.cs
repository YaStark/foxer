using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities.Descriptors;
using foxer.Core.Game.Items;
using System.Collections.Generic;

namespace foxer.Core.Game.Entities
{
    public class GrassWallEntityDescriptor : EntityDescriptor<GrassWallEntity>
    {
        public GrassWallEntityDescriptor()
            : base(EntityKind.BigItem)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IPlatform platform)
        {
            if(platform == stage.DefaultPlatform)
            {
                return cell.Kind == CellKind.Floor;
            }

            return true;
        }

        public override bool CheckCanOtherBePlacedHere(EntityDescriptorBase descriptor)
        {
            return true;
        }

        protected override ItemBase OnGetLoot(Stage stage, GrassWallEntity entity)
        {
            return stage.ItemManager.Create<ItemGrassWall>(stage, 1);
        }
    }
}
