using System;

namespace Assets.Scripts.Models.Food
{
    public class CoffeePack : ConsumableItem
    {
        public CoffeePack()
        {
            LocalizationName = "coffee_pack";
            IconName = "coffee_pack_icon";

            EnergyEffect = 25f;
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeEnergy(EnergyEffect);
        }
    }
}
