using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Decor
{
    public class Cupboard_Closed : Decor
    {
        public Cupboard_Closed()
        {
            LocalizationName = "item-cupboard_closed";
            IconName = "cupboard_closed_icon";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Decor/Cupboard_Closed/Cupboard_ClosedTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/Decor/Cupboard_Closed/Cupboard_Closed";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 50));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 10));
        }
    }
}