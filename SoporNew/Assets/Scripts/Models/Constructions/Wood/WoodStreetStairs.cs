using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Constructions.Wood
{
    public class WoodStreetStairs : Construction
    {
        public WoodStreetStairs()
        {
            ConstructionType = ConstructionType.Wall;

            LocalizationName = "wood_street_stairs";
            Description = "wood_street_stairs_descr";
            IconName = "wood_street_stairs_icon";

            PrefabPath = "Prefabs/Constructions/Wood/StreetStairs/StreetStairs";
            PrefabTemplatePath = "Prefabs/Constructions/Wood/StreetStairs/StreetStairsTemplate";

            CraftRecipe = new List<HolderObject>();
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 30));
        }
    }
}
