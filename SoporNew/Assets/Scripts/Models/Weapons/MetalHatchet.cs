using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class MetalHatchet : Weapon
    {
        public MetalHatchet()
        {
            WeaponType = WeaponType.Melee;
            WeaponInventoryId = 13;
            LocalizationName = "metal_hatchet";
            Description = "metal_hatchet_descr";
            IconName = "metal_hatchet_icon";
            Durability = 250;
            Damage = 50;
            MiningAdditionalAmount = 1;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 50));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 30));
        }
    }
}
