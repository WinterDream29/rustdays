using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Constructions.Stone
{
    public class StoneCeiling : Construction
    {
        public StoneCeiling()
        {
            ConstructionType = ConstructionType.Ceiling;

            LocalizationName = "stone_ceiling";
            Description = "stone_ceiling_descr";
            IconName = "stone_ceiling_icon";

            PrefabPath = "Prefabs/Constructions/Stone/Ceiling/StoneCeiling";
            PrefabTemplatePath = "Prefabs/Constructions/Stone/Ceiling/StoneCeilingTemplate";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 10));
			CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(StoneResource), 30));
        }
    }
}
