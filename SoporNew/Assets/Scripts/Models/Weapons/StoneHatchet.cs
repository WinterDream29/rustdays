using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class StoneHatchet : Weapon
    {
        public StoneHatchet()
        {
            WeaponType = WeaponType.Melee;
            WeaponInventoryId = 5;
            LocalizationName = "stone_hatchet";
            Description = "stone_hatchet_descr";
            IconName = "stone_hatchet_icon";
            Durability = 100;
            Damage = 40;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 20));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(StoneResource), 4));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 2));
        }
    }
}
