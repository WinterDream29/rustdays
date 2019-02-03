using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Common
{
    public class Barrel : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public Barrel()
        {
            LocalizationName = "barrel";
            Description = "barrel_descr";
            IconName = "barrel_icon";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Barrel/BarrelTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/Barrel/Barrel";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 60));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 30));
        }
    }
}
