using System;

namespace Assets.Scripts.Models.Seedlings.Watermelon
{
    public class WatermelonWithered : Food.Food
    {
        public WatermelonWithered()
        {
            LocalizationName = "watermelon_withered";
            IconName = "watermelon_withered_icon";

            HungerEffect = 2f;
            ThirstEffect = 15f;
            HealthEffect = -10f;
        }
        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeHunger(HungerEffect);
            gameManager.PlayerModel.ChangeThirst(ThirstEffect);
            gameManager.PlayerModel.ChangeHealth(HealthEffect);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerEat);
        }
    }
}
