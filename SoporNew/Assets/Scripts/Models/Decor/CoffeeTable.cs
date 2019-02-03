using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Decor
{
    public class CoffeeTable : Decor
    {
        public CoffeeTable()
        {
            LocalizationName = "coffee_table";
            IconName = "coffee_table_icon";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Decor/CoffeeTable/CoffeeTableTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/Decor/CoffeeTable/CoffeeTable";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 40));
        }
    }
}
