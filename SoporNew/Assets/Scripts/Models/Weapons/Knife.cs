using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class Knife : Weapon
    {
        public Knife()
        {
            WeaponType = WeaponType.Melee;
            WeaponInventoryId = 7;
            LocalizationName = "knife";
            Description = "knife_descr";
            IconName = "knife_icon";
            Durability = 100;
            Damage = 30;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 10));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 10));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 1));
        }
    }
}
