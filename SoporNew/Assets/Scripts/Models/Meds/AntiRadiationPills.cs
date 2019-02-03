using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Meds
{
    public class AntiRadiationPills : Medicines
    {
        public AntiRadiationPills()
        {
            LocalizationName = "anti_radiation_pills";
            Description = "anti_radiation_pills_descr";
            IconName = "anti_radiation_pills_icon";

            HealthEffect = 25f;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Bandage), 3));
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeHealth(HungerEffect);
        }
    }
}
