namespace Assets.Scripts.Models.ResourceObjects.CraftingResources
{
    public class Metal : ResourceObject
    {
        public Metal()
        {
            LocalizationName = "metal";
            Description = "metal_descr";
            IconName = "metal_icon";
            CanBuy = true;
            ShopPrice = 5;
        }
    }
}
