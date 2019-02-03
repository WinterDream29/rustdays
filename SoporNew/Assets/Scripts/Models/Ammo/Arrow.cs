using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Models.Ammo
{
    public class Arrow : Ammo
    {
        public Arrow()
        {
            LocalizationName = "arrow";
            Description = "arrow_descr";
            IconName = "arrow_icon";
            CraftAmount = 5;

            Damage = 80;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 20));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(StoneResource), 10));
        }
    }
}
