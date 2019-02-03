using System;

namespace Assets.Scripts.Models.Food
{
    public class Adrenaline : Food
    {
        public Adrenaline()
        {
            LocalizationName = "adrenaline";
            IconName = "adrenaline_icon";

            EnergyEffect = 50f;
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeEnergy(EnergyEffect);
        }
    }
}
