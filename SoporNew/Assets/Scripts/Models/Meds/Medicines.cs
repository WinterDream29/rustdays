using System;

namespace Assets.Scripts.Models.Meds
{
    public class Medicines : ConsumableItem
    {
        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            if (changeAmount != null)
                changeAmount(1);
        }
    }
}
