using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Clothes
{
    public class FurCap : Cap
    {
        public FurCap()
        {
            LocalizationName = "fur_cap";
            IconName = "fur_cap_icon";
            CraftAmount = 1;
            Durability = 15000;
            Effect = ItemEffectType.Damage;
            EffectAmount = 4;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Fur), 5));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 1));
        }
    }
}
