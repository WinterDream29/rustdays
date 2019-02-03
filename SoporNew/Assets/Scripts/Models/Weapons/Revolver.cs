using System.Collections.Generic;
using Assets.Scripts.Models.Ammo;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class Revolver : Weapon
    {
        public Revolver()
        {
            WeaponType = WeaponType.Fire;
            WeaponInventoryId = 2;
            WeaponInventoryName = "102_Revolver";
            AmmoInventoryName = "RevolverAmmo";
            AmmoType = typeof(RevolverBullet);
            MagazineCapacity = 6;
            LocalizationName = "revolver";
            Description = "fire_weapon";
            IconName = "revolver";
            Durability = 1500;
            CanZoom = true;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 40));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 150));
        }
    }
}
