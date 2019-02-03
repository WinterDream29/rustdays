using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Tools
{
    public class Torch : BaseObject
    {
        public Torch()
        {
            LocalizationName = "torch";
            Description = "torch_descr";
            IconName = "torch_icon";
            Durability = 300;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 10));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 1));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Fat), 3));
        }
    }
}
