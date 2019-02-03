using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Common
{
    public class Bed : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public Bed()
        {
            LocalizationName = "bed";
            Description = "bed_descr";
            IconName = "bed_icon";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Bed/BedTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/Bed/Bed";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 50));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 100));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Fur), 10));
        }
    }
}
