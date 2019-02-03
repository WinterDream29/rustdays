using System;

namespace Assets.Scripts.Models.Food
{
    public class FriedMeat : Food
    {
        public FriedMeat()
        {
            LocalizationName = "fried_meat";
            IconName = "fried_meat_icon";

            HungerEffect = 25f;
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeHunger(HungerEffect);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerEat);
        }
    }
}
