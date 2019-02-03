using Assets.Scripts.Models.ResourceObjects;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Common
{
    public class Chest : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public Chest()
        {
            LocalizationName = "chest";
            Description = "chest_descr";
            IconName = "chest_cion";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Chest/ChestTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/Chest/Chest";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 60));
        }
    }
}
