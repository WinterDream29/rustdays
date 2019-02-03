using System;

namespace Assets.Scripts.Models.Food
{
    public class Chocolate : Food
    {
        public Chocolate()
		{
            LocalizationName = "chocolate";
            Description = "chocolate_descr";
            IconName = "chocolate_icon";

			HungerEffect = 40.0f;
		}

		public override void Use(GameManager gameManager, Action<int> changeAmount = null)
		{
			base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeHunger(HungerEffect);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerEat);
		}
    }
}
