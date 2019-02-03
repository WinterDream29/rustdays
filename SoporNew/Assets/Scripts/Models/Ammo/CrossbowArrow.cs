using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Ammo
{
    public class CrossbowArrow : Ammo
    {
        public CrossbowArrow()
        {
            LocalizationName = "crossbow_arrow";
            Description = "crossbow_arrow_descr";
            IconName = "crossbow_arrow_icon";
            CraftAmount = 5;

            Damage = 120;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 20));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 10));
        }
    }
}
