using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class Crowbar : Weapon
    {
        public Crowbar()
        {
            WeaponType = WeaponType.Melee;
            WeaponInventoryId = 9;
            LocalizationName = "crowbar";
            IconName = "crowbar_icon";
            Durability = 300;
            Damage = 30;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 60));
        }
    }
}
