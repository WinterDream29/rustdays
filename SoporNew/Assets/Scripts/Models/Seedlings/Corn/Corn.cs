using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Seedlings.Corn
{
    public class Corn : Food.Food
    {
        public Corn()
        {
            LocalizationName = "corn";
            IconName = "corn_icon";

            HungerEffect = 15f;

            NeedWoodToCook = 2;
            Converters = new List<ItemConverterType> { ItemConverterType.Campfire };
            CookingResult = new KeyValuePair<BaseObject, int>(new Popcorn(), 1);
        }
        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeHunger(HungerEffect);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerEat);
        }
    }
}
