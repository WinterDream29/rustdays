using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using Assets.Scripts.Models.Weapons;

namespace Assets.Scripts.Models.Ammo
{
    public class DragunovAmmo : Ammo
    {
        public DragunovAmmo()
        {
            WeaponType = typeof(DragunobSniperRifle);
            LocalizationName = "dragunov_ammo";
            Description = "dragunov_ammo_descr";
            IconName = "dragunov_ammo";
            CraftAmount = 5;

            Damage = 60;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Gunpowder), 3));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 5));
        }
    }
}
