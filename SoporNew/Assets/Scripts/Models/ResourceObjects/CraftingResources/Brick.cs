namespace Assets.Scripts.Models.ResourceObjects.CraftingResources
{
    public class Brick : ResourceObject
    {
        public Brick()
        {
            LocalizationName = "brick";
            Description = "brick_descr";
            IconName = "brick_icon";
            CanBuy = true;
            ShopPrice = 3;
        }
    }
}
