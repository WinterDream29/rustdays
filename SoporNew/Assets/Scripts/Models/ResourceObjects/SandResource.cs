using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.ResourceObjects
{
    public class SandResource : ResourceObject
    {
        public SandResource()
        {
            LocalizationName = "sand_resource";
            IconName = "sand_icon";
            CanBuy = true;
            ShopPrice = 1;

            Converters = new List<ItemConverterType> { ItemConverterType.Furnace };
            CookingResult = new KeyValuePair<BaseObject, int> ( new GlassFragments(), 3);

            NeedWoodToCook = 2;
        }
    }
}
