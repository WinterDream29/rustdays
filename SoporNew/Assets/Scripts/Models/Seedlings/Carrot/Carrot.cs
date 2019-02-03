using System;
namespace Assets.Scripts.Models.Seedlings.Carrot
{
    public class Carrot : Food.Food
    {
        public Carrot()
        {
            LocalizationName = "carrot";
            IconName = "carrot_icon";

            HungerEffect = 10f;
        }
        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeHunger(HungerEffect);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerEat);
        }
    }
}
