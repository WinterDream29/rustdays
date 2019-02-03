using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Weapons
{
    public class SmallStoneHatchet : Weapon
    {
        public SmallStoneHatchet()
        {
            WeaponType = WeaponType.Melee;
            WeaponInventoryId = 11;
            LocalizationName = "small_stone_hatchet";
            Description = "stone_hatchet_descr";
            IconName = "small_stone_hatchet_icon";
            Durability = 60;
            Damage = 20;
            MiningAdditionalAmount = -1;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 4));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(StoneResource), 2));
        }
    }
}
