using Assets.Scripts.Models.ResourceObjects;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Common
{
    public class CampFire : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public CampFire()
        {
            LocalizationName = "camp_fire";
            Description = "camp_fire_descr";
            IconName = "camp_fire_icon";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/CampFire/CampFireTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/CampFire/CampFire";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 30));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(StoneResource), 15));
        }
    }
}
