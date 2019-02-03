using System.Collections.Generic;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using System;

namespace Assets.Scripts.Models.Weapons
{
    public class Bow : Weapon
    {
        public Bow()
        {
            WeaponType = WeaponType.Fire;
            WeaponInventoryId = 12;
            LocalizationName = "bow";
            Description = "bow_descr";
            IconName = "bow_icon";
            Durability = 500;

            CraftRecipe = new List<HolderObject>();
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(WoodResource), 30));
            CraftRecipe.Add(HolderObjectFactory.GetItem(typeof(Rope), 10));
        }

        public override void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            base.Use(gameManager, changeAmount);
            if (gameManager.Player.Torch.IsActive)
                gameManager.Player.Torch.Hide();
        }
    }
}
