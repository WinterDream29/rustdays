using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class Shovel : Weapon
    {
        public Shovel()
        {
            WeaponType = WeaponType.Melee;
            WeaponInventoryId = 15;
            LocalizationName = "shovel";
            Description = "shovel_descr";
            IconName = "shovel_icon";
            Durability = 250;
            Damage = 50;
            AttackFirstHalfTime = 0.6f;
            AttackSeconfHalfTime = 1.2f;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 30));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 40));
        }
    }
}
