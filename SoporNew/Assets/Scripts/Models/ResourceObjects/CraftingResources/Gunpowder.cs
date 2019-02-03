using System.Collections.Generic;

namespace Assets.Scripts.Models.ResourceObjects.CraftingResources
{
    public class Gunpowder : ResourceObject
    {
        public Gunpowder()
        {
            LocalizationName = "gunpowder";
            Description = "gunpowder_descr";
            IconName = "gunpowder";
            CanBuy = true;
            ShopPrice = 30;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Sulfur), 4));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Charcoal), 4));
        }
    }
}
