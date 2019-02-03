using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Decor
{
    public class BearRug : Decor
    {
        public BearRug()
        {
            LocalizationName = "bear_rug";
            IconName = "bear_rug_icon";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Decor/Rugs/BearRug/BearRugTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/Decor/Rugs/BearRug/BearRug";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Fur), 20));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Leather), 10));
        }
    }
}
