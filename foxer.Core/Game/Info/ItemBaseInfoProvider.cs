using foxer.Core.Game.Items;

namespace foxer.Core.Game.Info
{
    public class ItemBaseInfoProvider<TItem> : IItemInfoProvider
        where TItem : ItemBase
    {
        private readonly string _defaultName;
        private readonly string _defaultDescription;

        public ItemBaseInfoProvider(
            string defaultName,
            string defaultDescription)
        {
            _defaultName = defaultName;
            _defaultDescription = defaultDescription;
        }

        public string GetName(object item, Stage stage)
        {
            if(item is TItem tItem)
            {
                return $"{_defaultName} ({tItem.Count})";
            }

            return _defaultName;
        }

        public string GetDescription(object item, Stage stage)
        {
            return _defaultDescription;
        }
    }
}
