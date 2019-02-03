using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Common
{
    public class TorchOnWall : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public TorchOnWall()
        {
            LocalizationName = "torch_on_wall";
            Description = "torch_on_wall_descr";
            IconName = "torch_on_wall_icon";
            IsStackable = false;
            AddDestroyReward = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/TorchOnWall/TorchOnWallTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/TorchOnWall/TorchOnWall";

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 20));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Fat), 10));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 10));
        }
    }
}
