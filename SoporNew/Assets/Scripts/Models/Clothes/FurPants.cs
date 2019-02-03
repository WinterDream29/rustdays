using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Clothes
{
    public class FurPants : Pants
    {
        public FurPants()
        {
            LocalizationName = "fur_pants";
            IconName = "fur_pants_icon";
            CraftAmount = 1;
            Durability = 18000;
            Effect = ItemEffectType.Damage;
            EffectAmount = 8;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Fur), 8));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 4));
        }
    }
}
