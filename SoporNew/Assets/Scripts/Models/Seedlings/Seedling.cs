namespace Assets.Scripts.Models.Seedlings
{
    public class Seedling : Food.Food
    {
        public Seedling()
        {
            CanBuy = true;
            ShowDurability = false;
            Durability = 1000;
            GardenTimeStage1 = 1000;
            GardenTimeStage2 = 800;
            GardenTimeStage3 = 400;
        }
    }
}
