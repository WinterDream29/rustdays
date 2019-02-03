using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Common
{
    public class GardenBedBrick : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public GardenBedBrick()
        {
            LocalizationName = "garden_bed";
            Description = "garden_bed_descr";
            IconName = "garden_bed_brick";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/GardenBed/GardenBedBrickTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/GardenBed/GardenBedBrick";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Brick), 30));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Manure), 20));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(GroundResource), 30));
        }
    }
}
