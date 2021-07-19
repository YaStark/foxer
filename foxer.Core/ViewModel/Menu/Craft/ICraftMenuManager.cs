using foxer.Core.Game.Craft;

namespace foxer.Core.ViewModel.Menu.Craft
{
    public interface ICraftMenuManager
    {
        ItemCraftBase Selected { get; set; }
        CrafterBase Crafter { get; }
    }
}
