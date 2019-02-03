using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Clothes.Backpack
{
    public class BigBackpack : Backpack
    {
        public BigBackpack()
        {
            LocalizationName = "big_backpack";
            Description = "big_backpack_descr";
			IconName = "backpack_icon";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Fur), 15));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Leather), 20));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 10));
        }
    }
}
