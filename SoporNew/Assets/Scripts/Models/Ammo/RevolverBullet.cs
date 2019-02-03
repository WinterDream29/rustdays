using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using Assets.Scripts.Models.Weapons;

namespace Assets.Scripts.Models.Ammo
{
    public class RevolverBullet : Ammo
    {
        public RevolverBullet()
        {
            WeaponType = typeof(Revolver);
            LocalizationName = "revolver_ammo";
            Description = "revolver_ammo_descr";
            IconName = "revolver_ammo";
            CraftAmount = 5;

            Damage = 80;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Gunpowder), 2));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 3));
        }
    }
}
