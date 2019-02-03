using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Constructions.Wood
{
    public class WoodCeiling : Construction
    {
        public WoodCeiling()
        {
            ConstructionType = ConstructionType.Ceiling;

            LocalizationName = "wood_ceiling";
            Description = "wood_ceiling_descr";
            IconName = "wood_ceiling_icon";

            PrefabPath = "Prefabs/Constructions/Wood/Ceiling/WoodCeiling";
            PrefabTemplatePath = "Prefabs/Constructions/Wood/Ceiling/WoodCeilingTemplate";

            CraftRecipe = new List<HolderObject>();
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 40));
        }
    }
}
