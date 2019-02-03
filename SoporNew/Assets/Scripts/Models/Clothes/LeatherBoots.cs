using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Clothes
{
    public class LeatherBoots : Boots
    {
        public LeatherBoots()
        {
            LocalizationName = "leather_boots";
            IconName = "leather_boots_icon";
            CraftAmount = 1;
            Durability = 20000;
            Effect = ItemEffectType.Damage;
            EffectAmount = 6;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Leather), 4));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 2));
        }
    }
}
