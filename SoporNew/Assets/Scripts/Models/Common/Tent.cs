using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Common
{
    public class Tent : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public Tent()
        {
            LocalizationName = "tent";
            Description = "tent_descr";
            IconName = "tent_icon";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Tent/TentTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/Tent/Tent";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 5));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 20));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Leather), 15));
        }
    }
}
