using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Clothes
{
    public class LeatherCap : Cap
    {
        public LeatherCap()
        {
            LocalizationName = "leather_cap";
            IconName = "leather_cap_icon";
            CraftAmount = 1;
            Durability = 15000;
            Effect = ItemEffectType.Damage;
            EffectAmount = 5;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Leather), 5));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 2));
        }
    }
}
