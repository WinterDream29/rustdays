using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Constructions.Brick
{
    public class BrickFoundation : Construction
    {
        public BrickFoundation()
        {
            ConstructionType = ConstructionType.Foundation;

            LocalizationName = "brick_foundation";
            Description = "brick_foundation_descr";
            IconName = "brick_foundation_icon";

            PrefabPath = "Prefabs/Constructions/Brick/Foundation/BrickFoundation";
            PrefabTemplatePath = "Prefabs/Constructions/Brick/Foundation/BrickFoundationTemplate";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 10));
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(ResourceObjects.CraftingResources.Brick), 40));
        }
    }
}
