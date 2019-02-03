using System;

namespace Assets.Scripts.Models.Seedlings.Strawberry
{
    public class StrawberryWithered : Food.Food
    {
        public StrawberryWithered()
        {
            LocalizationName = "strawberry_withered";
            IconName = "strawberry_withered_icon";

            HungerEffect = 10f;
            HealthEffect = -8f;
        }
        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeHunger(HungerEffect);
            gameManager.PlayerModel.ChangeHealth(HealthEffect);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerEat);
        }
    }
}
