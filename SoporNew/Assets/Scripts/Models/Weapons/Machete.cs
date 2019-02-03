using Assets.Scripts.Models.ResourceObjects;
using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class Machete : Weapon 
    {
        public Machete()
        {
            WeaponType = WeaponType.Melee;
            WeaponInventoryId = 10;
            LocalizationName = "machete";
            Description = "machete_descr";
            IconName = "machete_icon";
            Durability = 300;
            Damage = 70;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 30));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 50));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 3));
        }
    }
}
