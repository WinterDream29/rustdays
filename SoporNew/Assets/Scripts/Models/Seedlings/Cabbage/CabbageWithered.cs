using System;

namespace Assets.Scripts.Models.Seedlings.Cabbage
{
    public class CabbageWithered : Food.Food
    {
        public CabbageWithered()
        {
            LocalizationName = "cabbage_withered";
            IconName = "cabbage_withered_icon";

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
