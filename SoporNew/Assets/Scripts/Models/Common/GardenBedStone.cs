using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Common
{
    public class GardenBedStone : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public GardenBedStone()
        {
            LocalizationName = "garden_bed";
            Description = "garden_bed_descr";
            IconName = "garden_bed_stone";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/GardenBed/GardenBedStoneTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/GardenBed/GardenBedStone";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(StoneResource), 30));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Manure), 20));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(GroundResource), 30));
        }
    }
}
