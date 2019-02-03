namespace Assets.Scripts
{
    public class IapGoodItem
    {
        public string LocalizedTitle;
        public string LocalizedDescription;
        public int Price;
        public string RewardItemName;
        public int RewardAmount;

        public IapGoodItem(string rewardItemName, int rewardItemAmount, int price, string title, string description)
        {
            RewardItemName = rewardItemName;
            RewardAmount = rewardItemAmount;
            Price = price;
            LocalizedTitle = title;
            LocalizedDescription = description;
        }
    }
}
