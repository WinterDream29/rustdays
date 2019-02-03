using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Common
{
    public class GardenBed : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public GardenBed()
        {
            LocalizationName = "garden_bed";
            Description = "garden_bed_descr";
            IconName = "garden_bed_icon";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/GardenBed/GardenBedTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/GardenBed/GardenBed";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 40));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Manure), 20));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(GroundResource), 30));
        }
    }
}
