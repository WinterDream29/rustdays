using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Seedlings.Strawberry
{
    public class StrawberrySeed : Seedling
    {
        public StrawberrySeed()
        {
            LocalizationName = "strawberry_seed";
            Description = "strawberry_seed_descr";
            IconName = "strawberry_seed_icon";
            GardenPlacedPrefabStage1 = "Prefabs/Environment/Seedling/Strawberry/StrawberryStage1";
            GardenPlacedPrefabStage2 = "Prefabs/Environment/Seedling/Strawberry/StrawberryStage2";
            GardenPlacedPrefabStage3 = "Prefabs/Environment/Seedling/Strawberry/StrawberryStage3";
            GardenPlacedPrefabWithered = "Prefabs/Environment/Seedling/Strawberry/StrawberryWithered";

            GardenResult = new KeyValuePair<BaseObject, int>(new Strawberry(), 1);
            GardenWitheredResult = new KeyValuePair<BaseObject, int>(new StrawberryWithered(), 1);

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
