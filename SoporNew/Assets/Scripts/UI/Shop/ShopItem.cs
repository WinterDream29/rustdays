using Assets.Scripts.Models;
using Assets.Scripts.Ui;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.UI.Shop
{
    public class ShopItem : View
    {
        public UILabel NameLabel;
        public UISprite IconSprite;
        public UILabel PriceLabel;
        public UILabel AmountLabel;
        public GameObject BuyButton;

        private BaseObject _item;
        private IapGoodItem IapItem;
        private ShopView _shop;
        public GameObject CloneAnimateItem { get; set; }

        public void Init(GameManager gameManager, IapGoodItem iapItem, ShopView shop)
        {
            base.Init(gameManager);

            IapItem = iapItem;
            _item = BaseObjectFactory.GetItem(IapItem.RewardItemName);
            _shop = shop;

            NameLabel.text = Localization.Get(IapItem.LocalizedTitle);
            IconSprite.spriteName = _item.IconName;
            PriceLabel.text = IapItem.Price.ToString();
            AmountLabel.text = "x" + IapItem.RewardAmount;

            UIEventListener.Get(BuyButton).onClick += OnBuyClick;
        }

        public override void UpdateView()
        {
            NameLabel.text = Localization.Get(IapItem.LocalizedTitle);
        }

        private void OnBuyClick(GameObject go)
        {
            if (CurrencyManager.CurrentCurrency >= IapItem.Price)
            {
                CurrencyManager.AddCurrency(-IapItem.Price);
                var item = HolderObjectFactory.GetItem(_item.GetType(), IapItem.RewardAmount);
                var isAdd = GameManager.PlayerModel.Inventory.AddItem(item);
                if (!isAdd)
                    GameManager.PlacementItemsController.DropItemToGround(GameManager, item);
                StartCoroutine(AnimateBuyItem());
            }
            else
            {
                _shop.SelectCategory(ShopCategory.Gold);
                _shop.NoManyAnimate();
            }
        }

        private IEnumerator AnimateBuyItem()
        {
            CloneAnimateItem = Instantiate(IconSprite.gameObject);
            CloneAnimateItem.transform.position = IconSprite.gameObject.transform.position;
            CloneAnimateItem.transform.parent = IconSprite.gameObject.transform.parent;
            CloneAnimateItem.transform.localScale = Vector3.one;
            TweenScale.Begin(CloneAnimateItem, 0.25f, new Vector3(1.3f, 1.3f, 1.3f));
            yield return new WaitForSeconds(0.25f);
            TweenScale.Begin(CloneAnimateItem, 0.25f, new Vector3(1.1f, 1.1f, 1.1f));
            yield return new WaitForSeconds(0.3f);
            TweenPosition.Begin(CloneAnimateItem, 0.5f, new Vector3(IconSprite.gameObject.transform.position.x, IconSprite.gameObject.transform.position.y - 600f, IconSprite.gameObject.transform.position.z));
            yield return new WaitForSeconds(0.5f);
            Destroy(CloneAnimateItem);
            CloneAnimateItem = null;
        }
    }
}
