using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.ResourceObjects
{
    public class WoodResource : ResourceObject
    {
        public WoodResource()
        {
            LocalizationName = "wood_resource";
            IconName = "wood_icon";
            CanBuy = true;
            ShopPrice = 1;

            Converters = new List<ItemConverterType> { ItemConverterType.Furnace, ItemConverterType.Campfire };
            CookingResult = new KeyValuePair<BaseObject, int> ( new Charcoal(), 1);
        }
    }
}
