using System;

namespace Assets.Scripts.Models.Food
{
    public class Jerrycan : Food
    {
        public Jerrycan()
        {
            LocalizationName = "jerrycan";
            IconName = "jerrican_icon";

            FuelEffect = 0.5f;
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.CarInteractive.AddFuel(FuelEffect);
        }
    }
}
