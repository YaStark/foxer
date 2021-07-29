using foxer.Core.Game.Items;

namespace foxer.Core.Game.Info
{
    public class ItemBaseInfoProvider<TItem> : IItemInfoProvider
        where TItem : ItemBase
    {
        private readonly string _defaultName;

        public ItemBaseInfoProvider(string defaultName)
        {
            _defaultName = defaultName;
        }

        public string GetText(object item, Stage stage)
        {
            if(item is TItem tItem)
            {
                return $"{_defaultName} ({tItem.Count})";
            }

            return _defaultName;
        }
    }
}
