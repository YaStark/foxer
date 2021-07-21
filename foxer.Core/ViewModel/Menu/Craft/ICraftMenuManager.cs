using foxer.Core.Game.Craft;

namespace foxer.Core.ViewModel.Menu.Craft
{
    public interface ICraftMenuManager
    {
        ItemCraftBase SelectedCraft { get; set; }
        CrafterBase Crafter { get; }

        void SetSelectedRequirement(CraftRequirementsBase requirement);
    }
}
