using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Seedlings.Corn
{
    public class CornSeed : Seedling
    {
        public CornSeed()
        {
            LocalizationName = "corn_seed";
            Description = "corn_seed_descr";
            IconName = "corn_seed_icon";
            GardenPlacedPrefabStage1 = "Prefabs/Environment/Seedling/Corn/CornStage1";
            GardenPlacedPrefabStage2 = "Prefabs/Environment/Seedling/Corn/CornStage2";
            GardenPlacedPrefabStage3 = "Prefabs/Environment/Seedling/Corn/CornStage3";
            GardenPlacedPrefabWithered = "Prefabs/Environment/Seedling/Corn/CornWithered";

            GardenResult = new KeyValuePair<BaseObject, int>(new Corn(), 1);
            GardenWitheredResult = new KeyValuePair<BaseObject, int>(new CornWithered(), 1);

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
