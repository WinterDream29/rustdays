using System;

namespace Assets.Scripts.Models.Seedlings.Corn
{
    public class Popcorn : Food.Food
    {
        public Popcorn()
        {
            LocalizationName = "popcorn";
            IconName = "popcorn_icon";

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
