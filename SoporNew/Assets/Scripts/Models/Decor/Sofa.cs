using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Decor
{
    public class Sofa : Decor
    {
        public Sofa()
        {
            LocalizationName = "sofa";
            IconName = "sofa_icon";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Decor/Sofa/SofaTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/Decor/Sofa/Sofa";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 50));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Fur), 20));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Leather), 10));
        }
    }
}
