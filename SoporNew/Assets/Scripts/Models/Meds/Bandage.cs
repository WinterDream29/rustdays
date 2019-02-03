using System;
using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Meds
{
    public class Bandage : Medicines
    {
        public Bandage()
        {
            LocalizationName = "bandage";
            Description = "bandage_descr";
            IconName = "bandage_icon";

            HealthEffect = 10f;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Leather), 2));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(MedicinalPlant), 1));
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeHealth(HungerEffect);
        }
    }
}
