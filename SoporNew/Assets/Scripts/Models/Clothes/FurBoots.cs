using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Clothes
{
    public class FurBoots : Boots
    {
        public FurBoots()
        {
            LocalizationName = "fur_boots";
            IconName = "fur_boots_icon";
            CraftAmount = 1;
            Durability = 10000;
            Effect = ItemEffectType.Damage;
            EffectAmount = 5;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Fur), 6));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 2));
        }
    }
}
