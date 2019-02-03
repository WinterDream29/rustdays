using System;

namespace Assets.Scripts.Models.Seedlings.Carrot
{
    public class CarrotWithered : Food.Food
    {
        public CarrotWithered()
        {
            LocalizationName = "carrot_withered";
            IconName = "carrot_withered";

            HungerEffect = 7f;
            HealthEffect = -7f;
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
