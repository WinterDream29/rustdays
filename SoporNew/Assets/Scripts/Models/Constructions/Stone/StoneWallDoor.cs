using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Constructions.Stone
{
    public class StoneWallDoor : Construction
    {
        public StoneWallDoor()
        {
            ConstructionType = ConstructionType.Wall;

            LocalizationName = "stone_wall_door";
            Description = "stone_wall_door_descr";
            IconName = "stone_wall_door_icon";

            PrefabPath = "Prefabs/Constructions/Stone/WallDoor/StoneWallDoor";
            PrefabTemplatePath = "Prefabs/Constructions/Stone/WallDoor/StoneWallDoorTemplate";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 20));
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(StoneResource), 25));
        }
    }
}
