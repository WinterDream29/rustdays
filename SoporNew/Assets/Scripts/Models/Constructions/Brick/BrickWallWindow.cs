using System.Collections.Generic;
namespace Assets.Scripts.Models.Constructions.Brick
{
    public class BrickWallWindow : Construction
    {
        public BrickWallWindow()
        {
            ConstructionType = ConstructionType.Wall;

            LocalizationName = "brick_wall_window";
            Description = "brick_wall_window_descr";
            IconName = "brick_wall_window_icon";

            PrefabPath = "Prefabs/Constructions/Brick/WallWindow/BrickWallWindow";
            PrefabTemplatePath = "Prefabs/Constructions/Brick/WallWindow/BrickWallWindowTemplate";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(ResourceObjects.CraftingResources.GlassFragments), 10));
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(ResourceObjects.CraftingResources.Brick), 20));
        }
    }
}
