using System;
using System.Collections.Generic;
using Assets.Scripts.Models.Ammo;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class Shotgun : Weapon
    {
        public Shotgun()
        {
            WeaponType = WeaponType.Fire;
            WeaponInventoryId = 18;
            WeaponInventoryName = "118_Shotgun";
            AmmoInventoryName = "ShotgunAmmo";
            AmmoType = typeof(ShotgunAmmo);
            MagazineCapacity = 8;
            LocalizationName = "shotgun";
            Description = "fire_weapon";
            IconName = "shotgun";
            Durability = 2000;
            CanZoom = true;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 100));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 200));
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            if (gameManager.Player.Torch.IsActive)
                gameManager.Player.Torch.Hide();
        }
    }
}
