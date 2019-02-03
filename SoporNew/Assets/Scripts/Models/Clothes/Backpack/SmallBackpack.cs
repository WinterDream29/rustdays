using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Clothes.Backpack
{
    public class SmallBackpack : Backpack
    {
        public SmallBackpack()
        {
            LocalizationName = "small_backpack";
            Description = "small_backpack_descr";
			IconName = "backpack_icon";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Fur), 5));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Leather), 3));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 5));
        }
    }
}
