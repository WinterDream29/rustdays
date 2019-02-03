using System;

namespace Assets.Scripts.Models.Food
{
    public class FriedFish : Food
    {
        public FriedFish()
        {
            LocalizationName = "fried_fish";
            IconName = "grilled_fish_icon";

            HungerEffect = 20f;
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeHunger(HungerEffect);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerEat);
        }
    }
}
