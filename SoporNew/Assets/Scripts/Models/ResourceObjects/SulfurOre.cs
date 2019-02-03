using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.ResourceObjects
{
    public class SulfurOre : ResourceObject
    {
        public SulfurOre()
        {
            LocalizationName = "sulfur_ore";
            IconName = "sulfur_ore_icon";

            Converters = new List<ItemConverterType> { ItemConverterType.Furnace };
            CookingResult = new KeyValuePair<BaseObject, int> ( new Sulfur(), 3);
            NeedWoodToCook = 2;
        }
    }
}
