namespace foxer.Core.Game.Info
{
    public class StaticInfoProvider<TItem> : IItemInfoProvider
    {
        private readonly string _text;

        public StaticInfoProvider(string text)
        {
            _text = text;
        }

        public string GetText(object item, Stage stage)
        {
            return _text;
        }
    }
}
