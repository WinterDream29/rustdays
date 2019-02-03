using System;

namespace Assets.Scripts.Models.Seedlings.Strawberry
{
    public class Strawberry : Food.Food
    {
        public Strawberry()
        {
            LocalizationName = "strawberry";
            IconName = "strawberry_icon";

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
