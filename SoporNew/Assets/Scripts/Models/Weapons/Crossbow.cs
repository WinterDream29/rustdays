using System;
using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;

namespace Assets.Scripts.Models.Weapons
{
    public class Crossbow : Weapon
    {
        public Crossbow()
        {
            WeaponType = WeaponType.Fire;
            WeaponInventoryId = 17;
            LocalizationName = "crossbow";
            Description = "crossbow_descr";
            IconName = "crossbow_icon";
            Durability = 1000;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 30));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 10));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Metal), 100));
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            if (gameManager.Player.Torch.IsActive)
                gameManager.Player.Torch.Hide();
        }
    }
}
