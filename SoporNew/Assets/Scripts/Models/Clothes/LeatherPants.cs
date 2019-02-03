using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Clothes
{
    public class LeatherPants : Pants
    {
        public LeatherPants()
        {
            LocalizationName = "leather_pants";
            IconName = "leather_pants_icon";
            CraftAmount = 1;
            Durability = 18000;
            Effect = ItemEffectType.Damage;
            EffectAmount = 10;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Leather), 8));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 4));
        }
    }
}
