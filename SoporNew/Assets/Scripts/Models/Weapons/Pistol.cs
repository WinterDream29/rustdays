using System.Collections.Generic;
using Assets.Scripts.Models.Ammo;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class Pistol : Weapon
    {
        public Pistol()
        {
            WeaponType = WeaponType.Fire;
            WeaponInventoryId = 1;
            WeaponInventoryName = "101_Pistol";
            AmmoInventoryName = "PistolAmmo";
            AmmoType = typeof(PistolBullet);
            MagazineCapacity = 12;
            LocalizationName = "glock_17";
            Description = "fire_weapon";
            IconName = "pistol";
            Durability = 1000;
            CanZoom = true;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 30));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 100));
        }
    }
}
