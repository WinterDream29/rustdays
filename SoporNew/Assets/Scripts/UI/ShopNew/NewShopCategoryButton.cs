using System;
using Assets.Scripts.Ui;
using UnityEngine;

namespace Assets.Scripts.UI.ShopNew
{
    public enum NewShopCategory
    {
        Offers,
        Gold,
        Items
    }

    public class NewShopCategoryButton : View
    {
        public NewShopCategory Category;
        public UISprite SelectedSprite;

        private Action<NewShopCategory> _onClickAction;

        public void Init(GameManager gameManager, Action<NewShopCategory> onClickAction)
        {
            base.Init(gameManager);

            _onClickAction = onClickAction;

            UIEventListener.Get(gameObject).onClick += OnButtonClick;
        }

        private void OnButtonClick(GameObject go)
        {
            if (_onClickAction != null)
                _onClickAction(Category);
        }

        public void SetSelected(bool selected)
        {
            SelectedSprite.enabled = selected;
        }
    }
}