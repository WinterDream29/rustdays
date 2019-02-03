using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Constructions.Fence
{
    public class WoodenFence : Decor.Decor
    {
        public WoodenFence()
        {
            LocalizationName = "fence";
            IconName = "fence";

            PrefabTemplatePath = "Prefabs/Constructions/WoodenFence/WoodenFenceTemplate";
            PrefabPath = "Prefabs/Constructions/WoodenFence/WoodenFence";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 30));
        }
    }
}
