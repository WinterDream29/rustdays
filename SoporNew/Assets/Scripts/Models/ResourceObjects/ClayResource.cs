using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.ResourceObjects
{
    public class ClayResource : ResourceObject
    {
        public ClayResource()
        {
            LocalizationName = "clay_resource";
            IconName = "clay_icon";
            CanBuy = true;
            ShopPrice = 3;

            Converters = new List<ItemConverterType> { ItemConverterType.Furnace };
            CookingResult = new KeyValuePair<BaseObject, int> ( new Brick(), 2);

            NeedWoodToCook = 2;
        }
    }
}
