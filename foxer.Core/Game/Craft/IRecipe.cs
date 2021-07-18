using foxer.Core.Game.Items;
using System.Collections.Generic;

namespace foxer.Core.Game.Craft
{
    public interface IRecipe
    {
        int GetCraftTimeMs(Stage stage);
        bool CanCraft(Stage stage);
        bool BeginCraft();
        IEnumerable<ItemBase> GetResult();
    }
    
    public class RecipeStoneAxe : IRecipe
    {
        public bool BeginCraft()
        {
            throw new System.NotImplementedException();
        }

        public bool CanCraft(Stage stage)
        {
            return 
                stage.InventoryManager.CanAccomodate(new ItemStoneAxe());
        }

        public int GetCraftTimeMs(Stage stage)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ItemBase> GetResult()
        {
            throw new System.NotImplementedException();
        }
    }
}
