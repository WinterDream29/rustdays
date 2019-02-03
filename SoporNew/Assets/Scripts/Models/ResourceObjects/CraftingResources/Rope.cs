using System.Collections.Generic;
namespace Assets.Scripts.Models.ResourceObjects.CraftingResources
{
    public class Rope : ResourceObject
    {
        public Rope()
        {
            LocalizationName = "rope";
            Description = "rope_descr";
            IconName = "rope_icon";
            CanBuy = true;
            ShopPrice = 20;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(FiberResource), 3));
        }
    }
}
