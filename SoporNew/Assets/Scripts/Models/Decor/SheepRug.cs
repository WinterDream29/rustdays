using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Decor
{
    public class SheepRug : Decor
    {
        public SheepRug()
        {
            LocalizationName = "sheep_rug";
            IconName = "sheep_rug_icon";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Decor/Rugs/SheepRug/SheepRugTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/Decor/Rugs/SheepRug/SheepRug";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Fur), 15));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Leather), 8));
        }
    }
}
