using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using Assets.Scripts.Models.Weapons;

namespace Assets.Scripts.Models.Ammo
{
    public class ShotgunAmmo : Ammo
    {
        public ShotgunAmmo()
        {
            WeaponType = typeof(Shotgun);
            LocalizationName = "shotgun_ammo";
            Description = "shotgun_ammo_descr";
            IconName = "shotgun_ammo";
            CraftAmount = 4;

            Damage = 100;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Gunpowder), 4));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 6));
        }
    }
}