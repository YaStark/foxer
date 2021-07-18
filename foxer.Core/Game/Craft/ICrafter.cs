using foxer.Core.Game.Items;
using System.Collections.Generic;

namespace foxer.Core.Game.Craft
{
    public interface ICrafter
    {
        IList<ItemBase> SourceInventory { get; }
        IList<ItemBase> DestinationInventory { get; }
        IEnumerable<ICraftTask> ActiveQueue { get; }

        IEnumerable<ItemCraftBase> VisibleCrafts(Stage stage);
        IEnumerable<ItemCraftBase> AvailableCrafts(Stage stage);

        bool AddTask(ICraftTask task);
        void RemoveTask(ICraftTask task);
    }
}
