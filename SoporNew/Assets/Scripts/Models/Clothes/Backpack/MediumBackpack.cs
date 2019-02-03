using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Clothes.Backpack
{
    public class MediumBackpack : Backpack
    {
        public MediumBackpack()
        {
            LocalizationName = "medium_backpack";
            Description = "medium_backpack_descr";
			IconName = "backpack_icon";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Leather), 8));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 4));
        }
    }
}
