using System;
using System.Collections.Generic;
using Assets.Scripts.Models.Ammo;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class DragunobSniperRifle : Weapon
    {
        public DragunobSniperRifle()
        {
            WeaponType = WeaponType.Fire;
            WeaponInventoryId = 19;
            WeaponInventoryName = "119_DragunovSniperRifle";
            AmmoInventoryName = "DragunovAmmo";
            AmmoType = typeof(DragunovAmmo);
            MagazineCapacity = 10;
            LocalizationName = "dragunov_sniper_rifle";
            Description = "fire_weapon";
            IconName = "dragunov_sniper_rifle";
            Durability = 2500;
            CanZoom = true;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 150));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 300));
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            if (gameManager.Player.Torch.IsActive)
                gameManager.Player.Torch.Hide();
        }
    }
}
