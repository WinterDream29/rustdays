using System.Collections.Generic;
namespace Assets.Scripts.Models.ResourceObjects.CraftingResources
{
    public class WoodPlank : ResourceObject
    {
        public WoodPlank()
        {
            LocalizationName = "wood_plank";
            Description = "wood_plank_descr";
            IconName = "wood_plank_icon";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 3));
        }
    }
}
