using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Ui;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Purchasing;

namespace Assets.Scripts.UI.ShopNew
{
    public class NewShopView : View
    {
        public List<NewShopCategoryButton> SelectButtons;
        public NewShopCategoryItem CategoryItemPrefab;
        public NewShopCategoryIapItem CategoryIapItemPrefab;
        public NewShopRewardItem OfferRewardItemPrefab;
        public GameObject CategoryItemsRoot;
        public GameObject CategoryItemsDescrObject;
        public GameObject CategoryOfferItemsDescrObject;
        public GameObject CategoryGoldItemsDescrObject;
        public UIGrid CategoryIapItemsGrid;
        public UIGrid CategoryItemsGrid;
        public UIGrid OfferRewardItemsGrid;
        public GameObject CloseButton;
        public GameObject BuyButton;
        public GameObject RestorePursahceButton;
        public GameObject Panel;
        public UILabel CurrentBalanceLabel;
        public UISprite CurrentOfferItemIcon;
        public UILabel CurrentOfferItemDescr;
        public UILabel CurrentItemPrice;
        public UILabel CurrentItemPriceDescr;
        public UISprite CurrentGoldItemIcon;
        public UILabel CurrentGoldItemDescr;
        public UILabel CurrentGoldItemAmount;
        public GameObject TimeObject;
        public UILabel TimeLabel;

        private List<NewShopCategoryItem> _categoryItems;
        private List<NewShopCategoryIapItem> _categoryOffersItems;
        private List<NewShopCategoryIapItem> _categoryGoldItems;
        private List<NewShopRewardItem> _currentOfferRewardItems;
        private NewShopCategoryIapItem _currentIapItem;
        private float _tickTime = 0.0f;
        private bool _initialized;

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);

            foreach (var newShopCategoryButton in SelectButtons)
               newShopCategoryButton.Init(GameManager, OnCategorySelect);

            _categoryItems = new List<NewShopCategoryItem>();
            _categoryOffersItems = new List<NewShopCategoryIapItem>();
            _categoryGoldItems = new List<NewShopCategoryIapItem>();
            _currentOfferRewardItems = new List<NewShopRewardItem>();
            StartCoroutine(InitItems());
            UpdateBalance();
            CurrentItemPriceDescr.text = Localization.Get("buy");

            UIEventListener.Get(CloseButton).onClick += go => Hide();
            UIEventListener.Get(BuyButton).onClick += OnBuyClick;
            UIEventListener.Get(RestorePursahceButton).onClick += OnRestorePurshaceClick;
            CurrencyManager.OnChangeCurrency += UpdateBalance;
            GameManager.IapManager.OnBuyCurrency += CurrencyByued;

#if UNITY_ANDROID
            RestorePursahceButton.SetActive(false);
#endif
        }

        private void OnRestorePurshaceClick(GameObject go)
        {
            GameManager.IapManager.RestorePurchases(); 
        }

        private IEnumerator InitItems()
        {
            int i = 0;
            while (!GameManager.IapManager.IsInitialized())
            {
                yield return new WaitForSeconds(0.5f);
                i++;

                if(i > 30)
                    yield break;
            }

            AddShopItems();
            AddShopIapItems();

            CategoryItemsGrid.Reposition();
            CategoryIapItemsGrid.Reposition();

            _initialized = true;
        }

        private void AddShopItems()
        {
            foreach (IapGoodItem item in GameManager.IapManager.GoodItems)
            {
                var shopItem = NGUITools.AddChild(CategoryItemsGrid.gameObject, CategoryItemPrefab.gameObject).GetComponent<NewShopCategoryItem>();
                shopItem.Init(GameManager, item, this);
                shopItem.Show();
                _categoryItems.Add(shopItem);
            }
        }

        private void AddShopIapItems()
        {
            AddIapItem(IapStoreManager.BUY_1000_DOLLARS_ID, false);
            AddIapItem(IapStoreManager.BUY_5000_DOLLARS_ID, false);
            if (GameManager.IapManager.IsBuyFirst30000)
                AddIapItem(IapStoreManager.BUY_30000_DOLLARS_ID, false);
            else
                AddIapItem(IapStoreManager.BUY_30000_DOLLARS_ID, false, IapStoreManager.BUY_30000_DOLLARS_FIRST_ID, true);
            AddIapItem(IapStoreManager.BUY_20000_DOLLARS_ID, false);
            AddIapItem(IapStoreManager.FREE_GOLD, false);

            AddIapItem(IapStoreManager.STARTER_PACK, true, null, false, 5);
            AddIapItem(IapStoreManager.WEAPON_PACK, true, null, false, 5);
            AddIapItem(IapStoreManager.FIGHTER_KIT, true);
            AddIapItem(IapStoreManager.BUILDER_KIT, true);
            AddIapItem(IapStoreManager.SURVIVAL_KIT, true);

            if(!GameManager.IapManager.IsBuyNoAds)
                AddIapItem(IapStoreManager.NO_ADS, true);
        }

        private void AddIapItem(string iapItem, bool isOffer, string firstIapItem = null, bool isFirst = false, int maxPerLineItems = 4)
        {
            var shopIapItem = NGUITools.AddChild(CategoryIapItemsGrid.gameObject, CategoryIapItemPrefab.gameObject).GetComponent<NewShopCategoryIapItem>();
            shopIapItem.Init(GameManager, this, iapItem, isOffer, firstIapItem, isFirst, maxPerLineItems);
            if(isOffer)
                _categoryOffersItems.Add(shopIapItem);
            else
                _categoryGoldItems.Add(shopIapItem);
        }

        private void OnBuyClick(GameObject go)
        {
            if (_currentIapItem.ID == IapStoreManager.FREE_GOLD)
            {
                ShowRewardedAd();
            }
            else
            {
                GameManager.IapManager.BuyProductId(_currentIapItem.IapItem.definition.id);
            }
        }

        public void ShowRewardedAd()
        {
            if (Advertisement.IsReady("rewardedVideo"))
            {
                var options = new ShowOptions { resultCallback = HandleShowResult };
                Advertisement.Show("rewardedVideo", options);
            }
        }

        private void HandleShowResult(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    CurrencyManager.AddCurrency(_currentIapItem.Definition.Currency);
                    break;
                case ShowResult.Skipped:
                    break;
                case ShowResult.Failed:
                    break;
            }
        }

        public void SelectIapItem(NewShopCategoryIapItem item)
        {
            foreach (var newShopCategoryIapItem in _categoryOffersItems)
            {
                newShopCategoryIapItem.Select(newShopCategoryIapItem == item);
            }

            foreach (var newShopCategoryIapItem in _categoryGoldItems)
            {
                newShopCategoryIapItem.Select(newShopCategoryIapItem == item);
            }

            if (item.IsOffer)
            {
                CurrentOfferItemIcon.spriteName = item.Definition.IconName;
                CurrentOfferItemDescr.text = Localization.Get(item.Definition.DetailDescription);
                CurrentItemPrice.text = item.IapItem.metadata.localizedPriceString;

                foreach (var currentOfferRewardItem in _currentOfferRewardItems)
                    Destroy(currentOfferRewardItem.gameObject);
                _currentOfferRewardItems.Clear();

                if (item.Definition.Items != null)
                {
                    OfferRewardItemsGrid.maxPerLine = item.MaxPerLineItems;
                    foreach (var definitionItem in item.Definition.Items)
                    {
                        var rewardItem = NGUITools.AddChild(OfferRewardItemsGrid.gameObject, OfferRewardItemPrefab.gameObject).GetComponent<NewShopRewardItem>();
                        rewardItem.Init(definitionItem.Key, definitionItem.Value);
                        _currentOfferRewardItems.Add(rewardItem);
                    }

                    if (item.Definition.Currency > 0)
                    {
                        var rewardItem = NGUITools.AddChild(OfferRewardItemsGrid.gameObject, OfferRewardItemPrefab.gameObject).GetComponent<NewShopRewardItem>();
                        rewardItem.Init("gold", item.Definition.Currency);
                        _currentOfferRewardItems.Add(rewardItem);
                    }

                    OfferRewardItemsGrid.Reposition();
                    StartCoroutine(DelayMake(() => OfferRewardItemsGrid.Reposition(), 0.05f));
                }

                TimeObject.SetActive(item.IapItem.definition.id == IapStoreManager.STARTER_PACK);
            }
            else
            {
                CurrentGoldItemIcon.spriteName = item.Definition.IconName;
                CurrentGoldItemDescr.text = Localization.Get(item.Definition.Description);

                if(item.ID == IapStoreManager.FREE_GOLD)
                    CurrentItemPrice.text = Localization.Get("free");
                else
                    CurrentItemPrice.text = item.IapItem.metadata.localizedPriceString;

                CurrentGoldItemAmount.text = item.Definition.Currency.ToString();
            }

            _currentIapItem = item;
        }

        private IEnumerator DelayMake(Action action, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            action();
        }

        public void Show(NewShopCategory category)
        {
            Panel.SetActive(true);
            IsShowing = true;

            SelectCategory(category);
        }

        private void OnCategorySelect(NewShopCategory newShopCategory)
        {
            SelectCategory(newShopCategory);
        }

        public void SelectCategory(NewShopCategory category)
        {
            foreach (var newShopCategoryButton in SelectButtons)
            {
                newShopCategoryButton.SetSelected(category == newShopCategoryButton.Category);
            }

            foreach (var shopItem in _categoryItems)
            {
                if (shopItem.CloneAnimateItem != null)
                {
                    shopItem.StopAllCoroutines();
                    Destroy(shopItem.CloneAnimateItem);
                }
            }

            BuyButton.SetActive(category != NewShopCategory.Items);

            switch (category)
            {
                case NewShopCategory.Offers:
                    CategoryOfferItemsDescrObject.SetActive(true);
                    CategoryGoldItemsDescrObject.SetActive(false);
                    CategoryItemsRoot.SetActive(true);
                    CategoryItemsDescrObject.SetActive(false);

                    foreach (var newShopCategoryIapItem in _categoryOffersItems)
                        newShopCategoryIapItem.Show();
                    foreach (var newShopCategoryIapItem in _categoryGoldItems)
                        newShopCategoryIapItem.Hide();

                    SelectIapItem(_categoryOffersItems[0]);
                    break;
                case NewShopCategory.Gold:
                    CategoryOfferItemsDescrObject.SetActive(false);
                    CategoryGoldItemsDescrObject.SetActive(true);
                    CategoryItemsRoot.SetActive(true);
                    CategoryItemsDescrObject.SetActive(false);

                    foreach (var newShopCategoryIapItem in _categoryOffersItems)
                        newShopCategoryIapItem.Hide();
                    foreach (var newShopCategoryIapItem in _categoryGoldItems)
                        newShopCategoryIapItem.Show();

                    SelectIapItem(_categoryGoldItems[0]);
                    break;
                case NewShopCategory.Items:
                    CategoryOfferItemsDescrObject.SetActive(false);
                    CategoryGoldItemsDescrObject.SetActive(false);
                    CategoryItemsRoot.SetActive(false);
                    CategoryItemsDescrObject.SetActive(true);
                    break;
            }

            CategoryIapItemsGrid.Reposition();
        }

        private void CurrencyByued(string id)
        {
            if (!IsShowing)
                return;

            SoundManager.PlaySFX(WorldConsts.AudioConsts.Reward);
            StartCoroutine(AnimateAddCaps());
        }

        private void UpdateBalance()
        {
            CurrentBalanceLabel.text = CurrencyManager.CurrentCurrency.ToString();
        }

        public void NoManyAnimate()
        {
            StartCoroutine(AnimateAddCaps());
        }

        private IEnumerator AnimateAddCaps()
        {
            TweenScale.Begin(CurrentBalanceLabel.gameObject, 0.5f, new Vector3(1.3f, 1.3f, 1.3f));
            yield return new WaitForSeconds(0.6f);
            TweenScale.Begin(CurrentBalanceLabel.gameObject, 0.5f, new Vector3(1.0f, 1.0f, 1.0f));

            yield break;
        }

        void Update()
        {
            if(!_initialized || _currentIapItem == null || !IsShowing)
                return;

            if (_currentIapItem.IapItem != null && _currentIapItem.IapItem.definition.id == IapStoreManager.STARTER_PACK)
            {
                _tickTime += Time.deltaTime;
                if (_tickTime >= 1.0f)
                {
                    TimeLabel.text = GameManager.IapManager.GetStarterPackTime();
                    _tickTime = 0.0f;
                }
            }
        }

        public override void Hide()
        {
            Panel.SetActive(false);
            IsShowing = false;
        }

        void OnDestroy()
        {
            if(CurrencyManager.OnChangeCurrency != null)
                CurrencyManager.OnChangeCurrency -= UpdateBalance;
            if(GameManager.IapManager.OnBuyCurrency != null)
                GameManager.IapManager.OnBuyCurrency -= CurrencyByued;
        }
    }
}