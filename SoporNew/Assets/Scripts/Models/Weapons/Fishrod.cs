using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class Fishrod : Weapon
    {
        public Fishrod()
        {
            WeaponType = WeaponType.Fire;
            WeaponInventoryId = 16;
            LocalizationName = "fishrod";
            Description = "fishrod_descr";
            IconName = "fishrod_icon";
            Durability = 500;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 30));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 15));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 10));
        }
    }
}
