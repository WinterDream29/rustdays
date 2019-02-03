using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class StonePick : Weapon
    {
        public StonePick()
        {
            WeaponType = WeaponType.Melee;
            WeaponInventoryId = 6;
            LocalizationName = "stone_pick";
            Description = "stone_pick_descr";
            IconName = "stone_pick_icon";
            Durability = 120;
            Damage = 50;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 20));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(StoneResource), 10));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 2));
        }
    }
}
