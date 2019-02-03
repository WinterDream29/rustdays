using System;

namespace Assets.Scripts.Models.Food
{
    public class Soda : Food
    {
        public Soda()
        {
            LocalizationName = "soda";
            Description = "soda_descr";
            IconName = "soda";

            ThirstEffect = 30.0f;
            HungerEffect = 5.0f;
            EnergyEffect = 10.0f;
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            gameManager.PlayerModel.ChangeThirst(ThirstEffect);
            gameManager.PlayerModel.ChangeHunger(HungerEffect);
            gameManager.PlayerModel.ChangeEnergy(EnergyEffect);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerDrink);
        }
    }
}