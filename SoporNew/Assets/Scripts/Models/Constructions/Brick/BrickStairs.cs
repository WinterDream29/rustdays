using System.Collections.Generic;

namespace Assets.Scripts.Models.Constructions.Brick
{
    public class BrickStairs : Construction
    {
        public BrickStairs()
        {
            ConstructionType = ConstructionType.Stairs;

            LocalizationName = "brick_stairs";
            Description = "brick_stairs_descr";
            IconName = "brick_stairs_icon";

            PrefabPath = "Prefabs/Constructions/Brick/Stairs/BrickStairs";
            PrefabTemplatePath = "Prefabs/Constructions/Brick/Stairs/BrickStairsTemplate";

            CraftRecipe = new List<HolderObject>();
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(ResourceObjects.CraftingResources.Brick), 20));
        }
    }
}
