using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Constructions.Stone
{
    public class StoneWallWindow : Construction
    {
        public StoneWallWindow()
        {
            ConstructionType = ConstructionType.Wall;

            LocalizationName = "stone_wall_window";
            Description = "stone_wall_window_descr";
            IconName = "stone_wall_window_icon";

            PrefabPath = "Prefabs/Constructions/Stone/WallWindow/StoneWallWindow";
            PrefabTemplatePath = "Prefabs/Constructions/Stone/WallWindow/StoneWallWindowTemplate";

            CraftRecipe = new List<HolderObject>();
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(StoneResource), 30));
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(GlassFragments), 10));
        }
    }
}
