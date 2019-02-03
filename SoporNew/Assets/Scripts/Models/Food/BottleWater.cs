using System;

namespace Assets.Scripts.Models.Food
{
    public class BottleWater : Food
    {
        public BottleWater()
		{
            LocalizationName = "bottle_water";
            Description = "bottle_water_descr";
            IconName = "bottle_water";

			ThirstEffect = 40.0f;
		}

		public override void Use(GameManager gameManager, Action<int> changeAmount = null)
		{
			base.Use(gameManager, changeAmount);
			gameManager.PlayerModel.ChangeThirst(ThirstEffect);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerDrink);
		}
    }
}
