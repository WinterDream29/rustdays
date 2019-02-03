using System.Collections.Generic;
namespace Assets.Scripts.Models.ResourceObjects.CraftingResources
{
    public class Cobblestone : ResourceObject
    {
        public Cobblestone()
        {
            LocalizationName = "cobblestone";
            Description = "cobblestone_descr";
            IconName = "cobblestone_icon";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(StoneResource), 3));
        }
    }
}
