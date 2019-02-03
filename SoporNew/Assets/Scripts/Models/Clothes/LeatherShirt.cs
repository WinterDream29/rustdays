using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Clothes
{
    public class LeatherShirt : Shirt
    {
        public LeatherShirt()
        {
            LocalizationName = "leather_shirt";
            IconName = "leather_shirt_icon";
            CraftAmount = 1;
            Durability = 20000;
            Effect = ItemEffectType.Damage;
            EffectAmount = 8;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Leather), 10));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 4));
        }
    }
}
