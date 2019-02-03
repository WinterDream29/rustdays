using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Constructions.Wood
{
    public class WoodWallDoor : Construction
    {
        public WoodWallDoor()
        {
            ConstructionType = ConstructionType.Wall;

            LocalizationName = "wood_wall_door";
            Description = "wood_wall_door_descr";
            IconName = "wood_wall_door_icon";

            PrefabPath = "Prefabs/Constructions/Wood/WallDoor/WoodWallDoor";
            PrefabTemplatePath = "Prefabs/Constructions/Wood/WallDoor/WoodWallDoorTemplate";

            CraftRecipe = new List<HolderObject>();
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 40));
        }
    }
}
