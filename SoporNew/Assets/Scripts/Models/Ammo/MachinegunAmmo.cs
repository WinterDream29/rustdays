using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using Assets.Scripts.Models.Weapons;

namespace Assets.Scripts.Models.Ammo
{
    public class MachinegunAmmo : Ammo
    {
        public MachinegunAmmo()
        {
            WeaponType = typeof(Machinegun);
            LocalizationName = "thompson_ammo";
            Description = "thompson_ammo_descr";
            IconName = "machinegun_bullet";
            CraftAmount = 10;

            Damage = 80;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Gunpowder), 2));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 3));
        }
    }
}
