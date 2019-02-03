using System;

namespace Assets.Scripts.Models.Ammo
{
    public class Ammo : BaseObject
    {
        public Type WeaponType { get; protected set; }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            if (changeAmount != null)
                changeAmount(1);
        }
    }
}
