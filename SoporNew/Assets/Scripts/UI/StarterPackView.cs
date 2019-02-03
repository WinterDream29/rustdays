using Assets.Scripts.Ui;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Assets.Scripts.UI
{
    public class StarterPackView : View
    {
        public GameObject BuyButton;
        public GameObject CloseButton;
        public UILabel PriceLabel;
        public UILabel OldPriceLabel;

        private Product _product;
        private Product _oldProduct;

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);

            UIEventListener.Get(BuyButton).onClick += OnBuyBiuttonClick;
            UIEventListener.Get(CloseButton).onClick += OnCloseClick;
        }

        public override void Show()
        {
            if (_product == null)
                _product = IapStoreManager.StoreController.products.WithID(IapStoreManager.STARTER_PACK);
            if(_oldProduct == null)
                _oldProduct = IapStoreManager.StoreController.products.WithID(IapStoreManager.STARTER_PACK_OLD);

            PriceLabel.text = _product.metadata.localizedPriceString;
            OldPriceLabel.text = _oldProduct.metadata.localizedPriceString;

            GameManager.IapManager.OnBuyCurrency += CurrencyByued;
            base.Show();
        }

        public override void Hide()
        {
            GameManager.IapManager.OnBuyCurrency -= CurrencyByued;
            base.Hide();
        }

        private void OnCloseClick(GameObject go)
        {
            Hide();
        }

        private void OnBuyBiuttonClick(GameObject go)
        {
            GameManager.IapManager.BuyProductId(_product.definition.id);
        }
        private void CurrencyByued(string id)
        {
            if (!IsShowing)
                return;

            Hide();
            SoundManager.PlaySFX(WorldConsts.AudioConsts.Reward);
        }
    }
}
