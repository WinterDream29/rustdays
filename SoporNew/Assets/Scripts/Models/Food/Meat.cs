using System;
using System.Collections.Generic;
namespace Assets.Scripts.Models.Food
{
    public class Meat : Food
    {
        public Meat()
        {
            LocalizationName = "meat";
            IconName = "meat_icon";

            HungerEffect = 15f;
            HealthEffect = -15f;
            NeedWoodToCook = 2;

            Converters = new List<ItemConverterType> { ItemConverterType.Campfire };
            CookingResult = new KeyValuePair<BaseObject, int> (new FriedMeat(), 1);
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeHunger(HungerEffect);
            gameManager.PlayerModel.ChangeHealth(HealthEffect);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerEat);
        }
    }
}
