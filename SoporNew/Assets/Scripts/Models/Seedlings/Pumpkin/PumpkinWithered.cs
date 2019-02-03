using System;

namespace Assets.Scripts.Models.Seedlings.Pumpkin
{
    public class PumpkinWithered : Food.Food
    {
        public PumpkinWithered()
        {
            LocalizationName = "pumpkin_withered";
            IconName = "pumpkin_withered_icon";

            HungerEffect = 15f;
            HealthEffect = -10f;
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
