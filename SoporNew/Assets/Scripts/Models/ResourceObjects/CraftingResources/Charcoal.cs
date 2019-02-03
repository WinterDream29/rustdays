namespace Assets.Scripts.Models.ResourceObjects.CraftingResources
{
    public class Charcoal : ResourceObject
    {
        public Charcoal()
        {
            LocalizationName = "charcoal";
            Description = "charcoal_descr";
            IconName = "charcoal_icon";
            CanBuy = true;
            ShopPrice = 3;
        }
    }
}
