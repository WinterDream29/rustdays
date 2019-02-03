using System.Collections.Generic;

namespace Assets.Scripts.Models.Constructions.Brick
{
    public class BrickWall : Construction
    {
        public BrickWall()
        {
            ConstructionType = ConstructionType.Wall;

            LocalizationName = "brick_wall";
            Description = "brick_wall_descr";
            IconName = "brick_wall_icon";

            PrefabPath = "Prefabs/Constructions/Brick/Wall/BrickWall";
            PrefabTemplatePath = "Prefabs/Constructions/Brick/Wall/BrickWallTemplate";

            CraftRecipe = new List<HolderObject>();
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(ResourceObjects.CraftingResources.Brick), 30));
        }
    }
}
