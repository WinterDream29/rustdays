using System.Collections.Generic;
using Assets.Scripts.Models.Ammo;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class Machinegun : Weapon
    {
        public Machinegun()
        {
            WeaponType = WeaponType.Fire;
            WeaponInventoryId = 3;
            WeaponInventoryName = "103_Machinegun";
            AmmoInventoryName = "MachinegunAmmo";
            AmmoType = typeof(MachinegunAmmo);
            MagazineCapacity = 40;
            LocalizationName = "thompson";
            Description = "fire_weapon";
            IconName = "machinegun";
            Durability = 2000;
            CanZoom = true;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 100));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 200));
        }
    }
}
