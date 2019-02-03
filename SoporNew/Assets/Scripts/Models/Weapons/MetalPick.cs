using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Weapons
{
    public class MetalPick : Weapon
    {
        public MetalPick()
        {
            WeaponType = WeaponType.Melee;
            WeaponInventoryId = 14;
            LocalizationName = "metal_pick";
            Description = "metal_pick_descr";
            IconName = "metal_pick_icon";
            Durability = 250;
            Damage = 60;
            MiningAdditionalAmount = 1;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 30));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 40));
        }
    }
}
