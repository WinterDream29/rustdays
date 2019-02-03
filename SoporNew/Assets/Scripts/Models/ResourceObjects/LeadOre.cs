using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.ResourceObjects
{
    public class LeadOre : ResourceObject
    {
        public LeadOre()
        {
            LocalizationName = "lead_ore";
            IconName = "lead_ore_icon";

            Converters = new List<ItemConverterType> { ItemConverterType.Furnace };
            CookingResult = new KeyValuePair<BaseObject, int> (new Lead(), 2);
            NeedWoodToCook = 5;
        }
    }
}
