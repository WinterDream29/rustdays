using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Meds
{
    public class Medkit : Medicines
    {
        public Medkit()
        {
            LocalizationName = "medkit";
            Description = "medkit_descr";
            IconName = "medkit_icon";

            HealthEffect = 50f;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Bandage), 5));
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeHealth(HealthEffect);
        }
    }
}
