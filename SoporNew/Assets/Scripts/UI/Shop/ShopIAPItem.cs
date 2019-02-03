using Assets.Scripts.Ui;
using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Assets.Scripts.UI.Shop
{
    public class ShopIAPItem : View
    {
        public UILabel NameLabel;
        public UILabel PriceLabel;
        public UISprite Icon;
        public UILabel TimeLabel;
        public GameObject FirstBuyObject;
        public GameObject BuyObject;
        public UILabel FirstPrice;
        public UILabel FirstOldPrice;

        public GameObject BuyButton;
        public GameObject BuyFirstButton;

        private Product _iapItem;
        private Product _firstIapItem;
        private bool _isFirst;
        private float _tickTime = 0.0f;

        public void Init(GameManager gameManager, Product iapItem, Product firstIapItem = null, bool isFirst = false)
        {
            base.Init(gameManager);

            _iapItem = iapItem;
            _firstIapItem = firstIapItem;
            _isFirst = isFirst;

            //NameLabel.text = _iapItem.metadata.localizedTitle;
            //Icon.spriteName = _iapItem.metadata.localizedDescription;
            PriceLabel.text = _iapItem.metadata.localizedPriceString;

            UIEventListener.Get(BuyButton).onClick += BuyClicked;

            if (BuyFirstButton != null)
                UIEventListener.Get(BuyFirstButton).onClick += BuyClicked;

            if (isFirst)
            {
                BuyObject.SetActive(false);
                FirstBuyObject.SetActive(true);
                FirstPrice.text = _firstIapItem.metadata.localizedPriceString;
                FirstOldPrice.text = _iapItem.metadata.localizedPriceString;
            }
        }

        void Update()
        {
            if (!_isFirst)
                return;

            if (!gameObject.activeInHierarchy)
                return;

            _tickTime += Time.deltaTime;
            if(_tickTime >= 1.0f)
            {
                _tickTime = 0.0f;
                TimeLabel.text = GameManager.IapManager.GetStockTime();
            }
        }

        public void BuyClicked(GameObject go)
        {
            if(_isFirst)
            {
                GameManager.IapManager.BuyProductId(_firstIapItem.definition.id);
                FirstBuyObject.SetActive(false);
                BuyObject.SetActive(true);
                _isFirst = false;
            }
            else
            {
                GameManager.IapManager.BuyProductId(_iapItem.definition.id);
            }
        }

        void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}
