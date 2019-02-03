using Assets.Scripts.Models.ResourceObjects;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Common
{
    public class InventoryBox : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public InventoryBox()
        {
            LocalizationName = "inventory_box";
            Description = "inventory_box_descr";
            IconName = "inventory_box_icon";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/InventoryBox/InventoryBoxTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/InventoryBox/InventoryBox";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 50));
        }
    }
}
