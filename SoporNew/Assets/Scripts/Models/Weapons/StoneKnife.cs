using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class StoneKnife : Weapon
    {
        public StoneKnife()
        {
            WeaponType = WeaponType.Melee;
            WeaponInventoryId = 7;
            LocalizationName = "stone_knife";
            Description = "stone_knife_descr";
            IconName = "stone_knife_icon";
            Durability = 50;
            Damage = 20;
            MiningAdditionalAmount = -1;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 10));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(StoneResource), 2));
        }
    }
}
