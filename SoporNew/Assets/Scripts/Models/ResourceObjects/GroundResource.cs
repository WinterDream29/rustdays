namespace Assets.Scripts.Models.ResourceObjects
{
    public class GroundResource : ResourceObject
    {
        public GroundResource()
        {
            LocalizationName = "ground_resource";
            IconName = "ground_icon";
            CanBuy = true;
            ShopPrice = 2;
        }
    }
}
