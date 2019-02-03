using System;
using System.Collections.Generic;
namespace Assets.Scripts.Models.Food
{
    public class Fish : Food
    {
        public Fish()
        {
            LocalizationName = "fish";
            IconName = "fish_icon";

            HungerEffect = 10f;

            Converters = new List<ItemConverterType> { ItemConverterType.Campfire };
            CookingResult = new KeyValuePair<BaseObject, int> ( new FriedFish(), 1);
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeHunger(HungerEffect);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerEat);
        }
    }
}
