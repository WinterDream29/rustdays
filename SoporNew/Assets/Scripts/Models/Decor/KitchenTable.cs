using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Decor
{
    public class KitchenTable : Decor
    {
        public KitchenTable()
        {
            LocalizationName = "kitchen_table";
            IconName = "kitchen_table_icon";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Decor/KitchenTable/KitchenTableTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/Decor/KitchenTable/KitchenTable";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 40));
        }
    }
}
