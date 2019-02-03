using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Seedlings.Pumpkin
{
    public class PumpkinSeed : Seedling
    {
        public PumpkinSeed()
        {
            LocalizationName = "pumpkin_seed";
            Description = "pumpkin_seed_descr";
            IconName = "pumpkin_seed_icon";
            GardenPlacedPrefabStage1 = "Prefabs/Environment/Seedling/Pumpkin/PumpkinStage1";
            GardenPlacedPrefabStage2 = "Prefabs/Environment/Seedling/Pumpkin/PumpkinStage2";
            GardenPlacedPrefabStage3 = "Prefabs/Environment/Seedling/Pumpkin/PumpkinStage3";
            GardenPlacedPrefabWithered = "Prefabs/Environment/Seedling/Pumpkin/PumpkinWithered";

            GardenResult = new KeyValuePair<BaseObject, int>(new Pumpkin(), 1);
            GardenWitheredResult = new KeyValuePair<BaseObject, int>(new PumpkinWithered(), 1);

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
