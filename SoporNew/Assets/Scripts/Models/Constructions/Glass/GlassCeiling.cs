using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Constructions.Glass
{
    public class GlassCeiling : Construction
    {
        public GlassCeiling()
        {
            ConstructionType = ConstructionType.Ceiling;

            LocalizationName = "glass_ceiling";
            Description = "glass_ceiling_descr";
            IconName = "glass_ceiling_icon";

            PrefabPath = "Prefabs/Constructions/Glass/Ceiling/GlassCeiling";
            PrefabTemplatePath = "Prefabs/Constructions/Glass/Ceiling/GlassCeilingTemplate";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(GlassFragments), 20));
        }
    }
}
