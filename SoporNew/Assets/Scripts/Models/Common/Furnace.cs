using Assets.Scripts.Models.ResourceObjects;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Common
{
    public class Furnace : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public Furnace()
        {
            LocalizationName = "furnace";
            Description = "furnace_descr";
            IconName = "furnace_icon";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Furnace/FurnaceTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/Furnace/Furnace";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 30));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(StoneResource), 40));
        }
    }
}
