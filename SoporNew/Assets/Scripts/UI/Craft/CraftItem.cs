using System;
using Assets.Scripts.Models;
using Assets.Scripts.Ui;
using UnityEngine;
using Assets.Scripts.Models.Clothes.Backpack;

namespace Assets.Scripts.UI.Craft
{
    public class CraftItem : View
    {
        public UISprite Icon;
        public UILabel AmountLabel;
        public UILabel Name;
        public UISprite CanCraftBackground;

        public BaseObject ItemModel { get; private set; }
        public CraftCategory Category { get; private set; }

        public Action<CraftItem> OnItemClickAction { get; set; }
        public bool CanCraft { get; private set; }

        private Type _itemType;

        public void Init(GameManager gameManager, CraftCategory category, Type itemType, string amount)
        {
            base.Init(gameManager);

            _itemType = itemType;
            ItemModel = BaseObjectFactory.GetItem(itemType);
            CanCraft = GameManager.PlayerModel.Inventory.CheckItems(ItemModel.CraftRecipe);
            Category = category;

            Icon.spriteName = ItemModel.IconName;
            AmountLabel.text = amount;
            if (Name != null)
                Name.text = Localization.Get(ItemModel.LocalizationName);
            if (CanCraftBackground != null)
                CanCraftBackground.enabled = CanCraft;

            UIEventListener.Get(gameObject).onClick += OnItemClick;
        }

        public override void UpdateView()
        {
            base.UpdateView();
            CanCraft = GameManager.PlayerModel.Inventory.CheckItems(ItemModel.CraftRecipe);
            if (GameManager.PlayerModel.CurrentBackpack == BackpackType.Big && ItemModel is BigBackpack)
                CanCraft = false;
            if (CanCraftBackground != null)
                CanCraftBackground.enabled = CanCraft;

            var amount = GameManager.PlayerModel.Inventory.GetAmount(_itemType);
            AmountLabel.text = amount.ToString();
        }

        private void OnItemClick(GameObject go)
        {
            if (OnItemClickAction != null)
                OnItemClickAction(this);
        }
    }
}
