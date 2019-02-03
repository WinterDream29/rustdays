using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class Mace : Weapon
    {
        public Mace()
        {
            WeaponType = WeaponType.Melee;
            WeaponInventoryId = 4;
            LocalizationName = "mace";
            IconName = "mace_icon";
            Durability = 200;
            Damage = 80;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 100));
        }
    }
}
