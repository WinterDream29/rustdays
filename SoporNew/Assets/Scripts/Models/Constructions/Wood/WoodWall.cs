using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Constructions.Wood
{
    public class WoodWall : Construction
    {
        public WoodWall()
        {
            ConstructionType = ConstructionType.Wall;

            LocalizationName = "wood_wall";
            Description = "wood_wall_descr";
            IconName = "wood_wall_icon";

            PrefabPath = "Prefabs/Constructions/Wood/Wall/WoodWall";
            PrefabTemplatePath = "Prefabs/Constructions/Wood/Wall/WoodWallTemplate";

            CraftRecipe = new List<HolderObject>();
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 40));
        }
    }
}
