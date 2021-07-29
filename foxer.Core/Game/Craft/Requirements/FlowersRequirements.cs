using foxer.Core.Game.Items;

namespace foxer.Core.Game.Craft
{
    public class FlowersRequirements : CraftResourceRequirementsBase
    {
        public FlowersRequirements(CrafterBase crafter, int count) 
            : base(crafter, typeof(ItemDandelion), count, typeof(ItemBlueFlower), typeof(ItemSunflower), typeof(ItemRedFlower))
        {
        }
    }
}
