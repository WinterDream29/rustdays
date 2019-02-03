using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Constructions.Stone
{
    public class StoneWall : Construction
    {
        public StoneWall()
        {
            ConstructionType = ConstructionType.Wall;

            LocalizationName = "stone_wall";
            Description = "stone_wall_descr";
            IconName = "stone_wall_icon";

            PrefabPath = "Prefabs/Constructions/Stone/Wall/StoneWall";
            PrefabTemplatePath = "Prefabs/Constructions/Stone/Wall/StoneWallTemplate";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 10));
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(StoneResource), 30));
        }
    }
}
