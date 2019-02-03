using System;

namespace Assets.Scripts.Models.Food
{
	public class Banana : Food
	{
		public Banana ()
		{
			LocalizationName = "banana";
			IconName = "banana_icon";

			HungerEffect = 10.0f;
		}

		public override void Use(GameManager gameManager, Action<int> changeAmount = null)
		{
			base.Use(gameManager, changeAmount);
			gameManager.PlayerModel.ChangeHunger(HungerEffect);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerEat);
		}
	}
}

