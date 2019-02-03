using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Constructions.Wood
{
    public class WoodStairs : Construction
    {
        public WoodStairs()
        {
            ConstructionType = ConstructionType.Stairs;

            LocalizationName = "wood_stairs";
            Description = "wood_stairs_descr";
            IconName = "Wooden_Stairs";

            PrefabPath = "Prefabs/Constructions/Wood/Stairs/WoodStairs";
            PrefabTemplatePath = "Prefabs/Constructions/Wood/Stairs/WoodStairsTemplate";

            CraftRecipe = new List<HolderObject>();
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 40));
        }
    }
}
