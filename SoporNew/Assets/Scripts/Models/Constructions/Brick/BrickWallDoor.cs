using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Constructions.Brick
{
    public class BrickWallDoor : Construction
    {
        public BrickWallDoor()
        {
            ConstructionType = ConstructionType.Wall;

            LocalizationName = "brick_wall_door";
            Description = "brick_wall_door_descr";
            IconName = "brick_wall_door_icon";

            PrefabPath = "Prefabs/Constructions/Brick/WallDoor/BrickWallDoor";
            PrefabTemplatePath = "Prefabs/Constructions/Brick/WallDoor/BrickWallDoorTemplate";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 20));
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(ResourceObjects.CraftingResources.Brick), 20));
        }
    }
}
