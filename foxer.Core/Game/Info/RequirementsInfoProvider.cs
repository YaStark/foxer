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

        public string GetText(object item, Stage stage)
        {
            if (item is CraftResourceRequirementsBase resReq
                && _itemInfoManager.TryGet(resReq.ItemType, out var info))
            {
                return $"{info.GetText(null, stage)} ({resReq.Crafter.Source.Count(resReq.ItemType)} / {resReq.Count})";
            }

            return string.Empty;
        }
    }
}
