using foxer.Core.Game.Entities.Descriptors;
using foxer.Core.Game.Items;

namespace foxer.Core.Game.Entities
{
    public class GrassFloorEntityDescriptor : PlatformEntityDescriptorBase<GrassFloorEntity>
    {
        protected override bool CheckCanOtherBePlacedHere(EntityDescriptorBase descriptor)
        {
            return descriptor != this;
        }

        protected override ItemBase OnGetLoot(Stage stage, GrassFloorEntity entity)
        {
            return stage.ItemManager.Create<ItemGrassFloor>(stage, 1);
        }
    }
}
