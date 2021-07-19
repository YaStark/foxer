namespace foxer.Core.Game.Craft
{
    public abstract class CraftRequirementsBase
    {
        public abstract bool Match(Stage stage);
        public abstract void Activate(Stage stage);
    }
}
