using System.Collections.Generic;
using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities.Descriptors;
using foxer.Core.Game.Items;

namespace foxer.Core.Game.Entities
{
    public class GrassRoofEntityDescriptor : EntityDescriptor<GrassRoofEntity>
    {
        public GrassRoofEntityDescriptor()
            : base(EntityKind.BigItem)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IPlatform platform)
        {
            return platform is IWall;
        }

        public override bool CheckCanOtherBePlacedHere(EntityDescriptorBase descriptor)
        {
            return false;
        }

        protected override ItemBase OnGetLoot(Stage stage, GrassRoofEntity entity)
        {
            return stage.ItemManager.Create<ItemGrassRoof>(stage, 1);
        }
    }
}
