using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Seedlings.Carrot
{
    public class CarrotSeed : Seedling
    {
        public CarrotSeed()
        {
            LocalizationName = "carrot_seed";
            Description = "carrot_seed_descr";
            IconName = "carrot_seed_icon";
            GardenPlacedPrefabStage1 = "Prefabs/Environment/Seedling/Carrot/CarrotStage1";
            GardenPlacedPrefabStage2 = "Prefabs/Environment/Seedling/Carrot/CarrotStage2";
            GardenPlacedPrefabStage3 = "Prefabs/Environment/Seedling/Carrot/CarrotStage3";
            GardenPlacedPrefabWithered = "Prefabs/Environment/Seedling/Carrot/CarrotWithered";

            GardenResult = new KeyValuePair<BaseObject, int>(new Carrot(), 1);
            GardenWitheredResult = new KeyValuePair<BaseObject, int>(new CarrotWithered(), 1);

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
