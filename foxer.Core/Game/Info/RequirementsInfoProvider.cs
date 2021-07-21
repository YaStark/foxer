using foxer.Core.Game.Craft;

namespace foxer.Core.Game.Info
{
    public class RequirementsInfoProvider : IItemInfoProviderWideTyped
    {
        private readonly ItemInfoManager _itemInfoManager;

        public RequirementsInfoProvider(ItemInfoManager itemInfoManager)
        {
            _itemInfoManager = itemInfoManager;
        }

        public bool Match<T>(T item)
        {
            return item is CraftRequirementsBase;
        }

        public string GetDescription(object item, Stage stage)
        {
            // todo add another types of requirements 

            if(item is CraftResourceRequirementsBase resReq)
            {
                return $"{resReq.Crafter.Source.Count(resReq.ItemType)} / {resReq.Count}";
            }

            return string.Empty;
        }

        public string GetName(object item, Stage stage)
        {
            if (item is CraftResourceRequirementsBase resReq
                && _itemInfoManager.TryGet(resReq.ItemType, out var info))
            {
                return $"{info.GetName(null, stage)}";
            }

            return string.Empty;
        }
    }
}
