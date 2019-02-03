namespace Assets.Scripts.Models.ResourceObjects
{
    public class StoneResource : ResourceObject
    {
        public StoneResource()
        {
            LocalizationName = "stone_resource";
            IconName = "stone_icon";
            CanBuy = true;
            ShopPrice = 2;
        }
    }
}
