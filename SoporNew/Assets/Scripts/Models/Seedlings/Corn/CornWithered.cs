using System;

namespace Assets.Scripts.Models.Seedlings.Corn
{
    public class CornWithered : Food.Food
    {
        public CornWithered()
        {
            LocalizationName = "corn_withered";
            IconName = "corn_wither_icon";

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
