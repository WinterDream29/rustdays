using System;

namespace Assets.Scripts.Models.Seedlings.Cabbage
{
    public class Cabbage : Food.Food
    {
        public Cabbage()
        {
            LocalizationName = "cabbage";
            IconName = "cabbage_icon";

            HungerEffect = 15f;
        }
        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeHunger(HungerEffect);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerEat);
        }
    }
}
