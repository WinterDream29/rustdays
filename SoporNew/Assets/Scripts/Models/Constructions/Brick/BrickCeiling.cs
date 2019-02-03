using System.Collections.Generic;

namespace Assets.Scripts.Models.Constructions.Brick
{
    public class BrickCeiling : Construction
    {
        public BrickCeiling()
        {
            ConstructionType = ConstructionType.Ceiling;

            LocalizationName = "brick_ceiling";
            Description = "brick_ceiling_descr";
            IconName = "brick_ceiling_icon";

            PrefabPath = "Prefabs/Constructions/Brick/Ceiling/BrickCeiling";
            PrefabTemplatePath = "Prefabs/Constructions/Brick/Ceiling/BrickCeilingTemplate";

            CraftRecipe = new List<HolderObject>();
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(ResourceObjects.CraftingResources.Brick), 30));
        }
    }
}
