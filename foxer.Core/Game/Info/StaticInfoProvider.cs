namespace foxer.Core.Game.Info
{
    public class StaticInfoProvider<TItem> : IItemInfoProvider
    {
        private readonly string _name;
        private readonly string _description;

        public StaticInfoProvider(string name, string description)
        {
            _name = name;
            _description = description;
        }

        public string GetDescription(object item, Stage stage)
        {
            return _description;
        }

        public string GetName(object item, Stage stage)
        {
            return _name;
        }
    }
}
