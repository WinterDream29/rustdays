using System;

namespace Assets.Scripts.Models.Seedlings.Watermelon
{
    public class Watermelon : Food.Food
    {
        public Watermelon()
        {
            LocalizationName = "watermelon";
            IconName = "watermelon_icon";

            HungerEffect = 5f;
            ThirstEffect = 20f;
        }
        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeHunger(HungerEffect);
            gameManager.PlayerModel.ChangeThirst(ThirstEffect);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerEat);
        }
    }
}
