using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Seedlings.Watermelon
{
    public class WatermelonSeed : Seedling
    {
        public WatermelonSeed()
        {
            LocalizationName = "watermelon_seed";
            Description = "watermelon_seed_descr";
            IconName = "watermelon_seed_icon";
            GardenPlacedPrefabStage1 = "Prefabs/Environment/Seedling/Watermelon/WatermelonStage1";
            GardenPlacedPrefabStage2 = "Prefabs/Environment/Seedling/Watermelon/WatermelonStage2";
            GardenPlacedPrefabStage3 = "Prefabs/Environment/Seedling/Watermelon/WatermelonStage3";
            GardenPlacedPrefabWithered = "Prefabs/Environment/Seedling/Watermelon/WatermelonWithered";

            GardenResult = new KeyValuePair<BaseObject, int>(new Watermelon(), 1);
            GardenWitheredResult = new KeyValuePair<BaseObject, int>(new WatermelonWithered(), 1);

            HungerEffect = 3.0f;
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeHunger(HungerEffect);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerEat);
        }
    }
}
