using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using Assets.Scripts.Models.Weapons;

namespace Assets.Scripts.Models.Ammo
{
    public class PistolBullet : Ammo
    {
        public PistolBullet()
        {
            WeaponType = typeof(Pistol);
            LocalizationName = "glock17_ammo";
            Description = "glock17_ammo_descr";
            IconName = "pistol_ammo";
            CraftAmount = 5;

            Damage = 40;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Gunpowder), 1));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 2));
        }
    }
}
