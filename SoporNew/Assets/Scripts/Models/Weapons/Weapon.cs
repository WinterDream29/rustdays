using System;
using UnityEngine;

namespace Assets.Scripts.Models.Weapons
{
    public enum WeaponType
    {
        Melee,
        Fire
    }

    public abstract class Weapon : UsableItem
    {
        public WeaponType WeaponType { get; protected set; }
        public int WeaponInventoryId { get; protected set; }
        public string WeaponInventoryName { get; protected set; }
        public string AmmoInventoryName { get; protected set; }
        public int MagazineCapacity{ get; protected set; }

        public Type AmmoType { get; protected set; }
        public bool CanZoom { get; protected set; }

        protected Weapon()
        {
            AttackFirstHalfTime = 0.4f;
            AttackSeconfHalfTime = 0.4f;
            ChangeWeaponTime = 2.0f;

            IsStackable = false;
            CanZoom = false;
            Description = "weapon_descr";
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);

            if (gameManager.Player.WeaponHandler.CurrentWeaponIndex != WeaponInventoryId)
                gameManager.Player.WeaponHandler.SetWeaponSmooth(WeaponInventoryId);

            if (AmmoType != null)
                SetAmmo(gameManager, true);

            gameManager.Player.MainHud.ZoomButton.SetActive(CanZoom);
            gameManager.Player.MainHud.AmmoLabel.enabled = AmmoType != null;

        }

        public virtual void SetAmmo(GameManager gameManager, bool isCurrentweapon = false)
        {
            var curAmmo = gameManager.PlayerModel.Inventory.GetAmount(AmmoType);
            var curMagazineAmount = (int)curAmmo / MagazineCapacity;
            var curAmount = (int)curAmmo % MagazineCapacity;
            gameManager.Player.EventHandler.SetItem.Try(new object[] { AmmoInventoryName, curMagazineAmount });
            gameManager.Player.EventHandler.SetAmmo.Try(new object[] { WeaponInventoryName, curAmount });
            if(isCurrentweapon)
                gameManager.Player.MainHud.UpdateAmmo(curAmount, MagazineCapacity);
        }
    }
}