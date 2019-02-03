using System;

namespace Assets.Scripts.Models.Food
{
	public class Apple : Food
	{
		public Apple ()
		{
			LocalizationName = "apple";
			IconName = "apple_icon";

			HungerEffect = 5.0f;
			ThirstEffect = 20.0f;
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

