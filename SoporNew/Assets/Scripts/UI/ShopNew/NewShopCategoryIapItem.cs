using Assets.Scripts.Ui;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Assets.Scripts.UI.ShopNew
{
    public class NewShopCategoryIapItem : View
    {
        public UISprite Icon;
        public UILabel DescriptionLabel;
        public UISprite SelectedBack;
        public GameObject Button;
        public GameObject OfferObject;
        public UILabel OfferPercentLabel;
        public UISprite HotDeal;
        public UISprite Free;
        public UISprite MostPopular;

        public string ID { get; private set; }
        public bool IsOffer { get; private set; }
        public IapItemDefinition Definition { get; private set; }
        public int MaxPerLineItems { get; private set; }
        public Product IapItem { get; private set; }

        private NewShopView _shop;
        private Product _firstIapItem;

        public void Init(GameManager gameManager, NewShopView shop, string iapItemId, bool isOffer, string firstIapItemId = null, bool isFirst = false, int maxPerLineItems = 4)
        {
            base.Init(gameManager);

            ID = iapItemId;
            IsOffer = isOffer;
            _shop = shop;
            MaxPerLineItems = maxPerLineItems;
            if(ID != IapStoreManager.FREE_GOLD)
                IapItem = IapStoreManager.StoreController.products.WithID(iapItemId);

            if (!string.IsNullOrEmpty(firstIapItemId))
                _firstIapItem = IapStoreManager.StoreController.products.WithID(firstIapItemId);

            Definition = GameManager.IapManager.IapItemDefinitions[iapItemId];
            if (isOffer)
            {
                DescriptionLabel.text = Localization.Get(Definition.Description);
                Icon.spriteName = Definition.IconName;
            }
            else
            {
                DescriptionLabel.text = Definition.Currency.ToString();
                Icon.spriteName = "gold_icon";
            }

            Free.enabled = ID == IapStoreManager.FREE_GOLD;

            SelectedBack.enabled = false;
            HotDeal.enabled = Definition.HotDeal;
            MostPopular.enabled = Definition.MostPopular;
            OfferObject.SetActive(Definition.OfferPercent > 0);
            OfferPercentLabel.text = "-" + Definition.OfferPercent + "%";

            UIEventListener.Get(Button).onClick += OnButtonClick;
        }

        private void OnButtonClick(GameObject go)
        {
            _shop.SelectIapItem(this);
        }

        public void Select(bool selected)
        {
            SelectedBack.enabled = selected;
        }
    }
}