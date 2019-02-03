using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class Catana : Weapon
    {
        public Catana()
        {
            WeaponType = WeaponType.Melee;
            WeaponInventoryId = 8;
            LocalizationName = "catana";
            Description = "catana_descr";
            IconName = "catana_icon";
            Durability = 200;
            Damage = 100;
            MiningAdditionalAmount = 1;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 20));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 50));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 3));
        }
    }
}
