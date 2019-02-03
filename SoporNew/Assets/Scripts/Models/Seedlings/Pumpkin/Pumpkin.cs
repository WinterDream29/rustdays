using System;

namespace Assets.Scripts.Models.Seedlings.Pumpkin
{
    public class Pumpkin : Food.Food
    {
        public Pumpkin()
        {
            LocalizationName = "pumpkin";
            IconName = "pumpkin_icon";

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
