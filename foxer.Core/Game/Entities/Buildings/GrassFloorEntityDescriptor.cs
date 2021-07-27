using foxer.Core.Game.Entities.Descriptors;

namespace foxer.Core.Game.Entities
{
    public class GrassFloorEntityDescriptor : PlatformEntityDescriptorBase<GrassFloorEntity>
    {
        protected override bool CheckCanOtherBePlacedHere(EntityDescriptorBase descriptor)
        {
            return descriptor != this;
        }
    }
}
