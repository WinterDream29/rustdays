using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Constructions.Stone
{
    public class StoneStairs : Construction
    {
        public StoneStairs()
        {
            ConstructionType = ConstructionType.Stairs;

            LocalizationName = "stone_stairs";
            Description = "stone_stairs_descr";
            IconName = "stone_stairs_icon";

            PrefabPath = "Prefabs/Constructions/Stone/Stairs/StoneStairs";
            PrefabTemplatePath = "Prefabs/Constructions/Stone/Stairs/StoneStairsTemplate";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 10));
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(StoneResource), 25));
        }
    }
}
