using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Constructions.Glass
{
    public class GlassWall : Construction
    {
        public GlassWall()
        {
            ConstructionType = ConstructionType.Wall;

            LocalizationName = "glass_wall";
            Description = "glass_wall_descr";
            IconName = "glass_wall_icon";

            PrefabPath = "Prefabs/Constructions/Glass/Wall/GlassWall";
            PrefabTemplatePath = "Prefabs/Constructions/Glass/Wall/GlassWallTemplate";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(GlassFragments), 20));
        }
    }
}
