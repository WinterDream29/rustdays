using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.ResourceObjects
{
    public class MetalOre : ResourceObject
    {
        public MetalOre()
        {
            LocalizationName = "metal_ore";
            IconName = "metal_ore_icon";

            Converters = new List<ItemConverterType>{ ItemConverterType.Furnace };
            CookingResult = new KeyValuePair<BaseObject, int> ( new Metal(), 5 );
            NeedWoodToCook = 3;
        }
    }
}
