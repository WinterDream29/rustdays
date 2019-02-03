using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Decor
{
    public class CupboardRoom : Decor
    {
        public CupboardRoom()
        {
            LocalizationName = "item-cupboard_room";
            IconName = "cupboard_room_icon";
            IsStackable = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Decor/CupboardRoom/CupboardRoomTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/Decor/CupboardRoom/CupboardRoom";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 50));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 10));
        }
    }
}