using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Constructions.Stone
{
    public class StoneFoundation : Construction
    {
        public StoneFoundation()
        {
            ConstructionType = ConstructionType.Foundation;

            LocalizationName = "stone_foundation";
            Description = "stone_foundation_descr";
            IconName = "stone_foundation_icon";

            PrefabPath = "Prefabs/Constructions/Stone/Foundation/StoneFoundation";
            PrefabTemplatePath = "Prefabs/Constructions/Stone/Foundation/StoneFoundationTemplate";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 20));
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(StoneResource), 40));
        }
    }
}
