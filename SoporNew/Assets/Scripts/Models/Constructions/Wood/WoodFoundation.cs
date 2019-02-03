using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Constructions.Wood
{
    public class WoodFoundation : Construction
    {
        public WoodFoundation()
        {
            ConstructionType = ConstructionType.Foundation;

            LocalizationName = "wood_foundation";
            Description = "wood_foundation_descr";
            IconName = "wood_foundation_icon";

            PrefabPath = "Prefabs/Constructions/Wood/Foundation/WoodFoundation";
            PrefabTemplatePath = "Prefabs/Constructions/Wood/Foundation/WoodFoundationTemplate";

            CraftRecipe = new List<HolderObject>();
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 60));
        }
    }
}
