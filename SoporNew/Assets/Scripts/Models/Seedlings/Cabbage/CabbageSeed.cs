using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Seedlings.Cabbage
{
    public class CabbageSeed : Seedling
    {
        public CabbageSeed()
        {
            LocalizationName = "cabbage_seed";
            Description = "cabbage_seed_descr";
            IconName = "cabbage_seed_icon";
            GardenPlacedPrefabStage1 = "Prefabs/Environment/Seedling/Cabbage/CabbageStage1";
            GardenPlacedPrefabStage2 = "Prefabs/Environment/Seedling/Cabbage/CabbageStage2";
            GardenPlacedPrefabStage3 = "Prefabs/Environment/Seedling/Cabbage/CabbageStage3";
            GardenPlacedPrefabWithered = "Prefabs/Environment/Seedling/Cabbage/CabbageWithered";

            GardenResult = new KeyValuePair<BaseObject, int>(new Cabbage(), 1);
            GardenWitheredResult = new KeyValuePair<BaseObject, int>(new CabbageWithered(), 1);

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
