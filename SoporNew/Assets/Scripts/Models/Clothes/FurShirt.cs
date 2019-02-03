using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Clothes
{
    public class FurShirt : Shirt
    {
        public FurShirt()
        {
            LocalizationName = "fur_shirt";
            IconName = "fur_shirt_icon";
            CraftAmount = 1;
            Durability = 14000;
            Effect = ItemEffectType.Damage;
            EffectAmount = 6;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Fur), 10));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 4));
        }
    }
}
