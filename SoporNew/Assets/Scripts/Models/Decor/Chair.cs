using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Decor
{
    public class Chair : Decor
    {
        public Chair()
        {
            LocalizationName = "chair";
            IconName = "chair_icon";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Decor/Chair/ChairTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/Decor/Chair/Chair";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 20));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Fur), 8));
        }
    }
}
