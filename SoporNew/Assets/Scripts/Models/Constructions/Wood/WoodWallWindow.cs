using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Constructions.Wood
{
    public class WoodWallWindow : Construction
    {
        public WoodWallWindow()
        {
            ConstructionType = ConstructionType.Wall;

            LocalizationName = "wood_wall_window";
            Description = "wood_wall_window_descr";
            IconName = "wood_wall_window_icon";

            PrefabPath = "Prefabs/Constructions/Wood/WallWindow/WoodWallWindow";
            PrefabTemplatePath = "Prefabs/Constructions/Wood/WallWindow/WoodWallWindowTemplate";

            CraftRecipe = new List<HolderObject>();
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 30));
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(GlassFragments), 10));
        }
    }
}
