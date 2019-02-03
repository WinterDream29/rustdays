using Assets.Scripts.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Assets.Scripts.UI.Shop
{
	public enum ShopCategory
	{
		Main,
		Gold
	}
	
    public class ShopView : View
    {
        public GameObject ShopItemPrefab;
        public GameObject ShopIAPItemPrefab;
        public UIScrollView ShopItemsScroll;
        public UIGrid ShopItemsGrid;
        public UIGrid ShopIAPItemsGrid;
        public UILabel CurrentBalanceLabel;
		public GameObject MainButton;
		public GameObject GoldButton;
		public Color ButtonActiveColor;
		public Color ButtonInactiveColor;
		public ShopIAPItem IapItem_1;
		public ShopIAPItem IapItem_2;
		public ShopIAPItem IapItem_3;
        public GameObject AdsButton;

        private List<ShopItem> _shopItems = new List<ShopItem>();
        private bool _isShopInitialized;

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);

            UpdateBalance();

			UIEventListener.Get (MainButton).onClick += OnMainButtonClick;
			UIEventListener.Get (GoldButton).onClick += OnGoldButtonClick;
            UIEventListener.Get(AdsButton).onClick += ShowRewardedAd;
            CurrencyManager.OnChangeCurrency += UpdateBalance;
            GameManager.IapManager.OnBuyCurrency += CurrencyByued;
        }

        public override void Show()
        {
            base.Show();
            InitializeShop();
            foreach (var item in _shopItems)
                item.UpdateView();
        }
        private void InitializeShop()
        {
            if (_isShopInitialized)
                return;

            AddShopItems();
            AddShopIapItems();
            SelectCategory(ShopCategory.Main);
            _isShopInitialized = true;
        }

        public void ShowRewardedAd(GameObject go)
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
                    CurrencyManager.AddCurrency(40);
                    ProgressManager.SaveProgress(GameManager);
                    SoundManager.PlaySFX(WorldConsts.AudioConsts.Reward);
                    StartCoroutine(AnimateAddCaps());
                    break;
                case ShowResult.Skipped:
                    Debug.Log("The ad was skipped before reaching the end.");
                    break;
                case ShowResult.Failed:
                    Debug.LogError("The ad failed to be shown.");
                    break;
            }
        }

		private void OnMainButtonClick(GameObject go)
		{
			SelectCategory (ShopCategory.Main);
		}

		private void OnGoldButtonClick(GameObject go)
		{
			SelectCategory (ShopCategory.Gold);
		}

		public void SelectCategory(ShopCategory category)
		{
		    if (category == ShopCategory.Gold)
		    {
		        foreach (var shopItem in _shopItems)
		        {
		            if (shopItem.CloneAnimateItem != null)
		            {
		                shopItem.StopAllCoroutines();
                        Destroy(shopItem.CloneAnimateItem);
		            }
		        }
		    }

            ShopItemsScroll.gameObject.SetActive(category == ShopCategory.Main);
			ShopIAPItemsGrid.gameObject.SetActive(category == ShopCategory.Gold);
			MainButton.GetComponentInChildren<UISprite> ().color = category == ShopCategory.Main ? ButtonActiveColor : ButtonInactiveColor;
			GoldButton.GetComponentInChildren<UISprite> ().color = category == ShopCategory.Gold ? ButtonActiveColor : ButtonInactiveColor;
            AdsButton.SetActive(category == ShopCategory.Gold);
		}

        private void UpdateBalance()
        {
            CurrentBalanceLabel.text = CurrencyManager.CurrentCurrency.ToString();
        }
        private void CurrencyByued(string id)
        {
            if (!IsShowing)
                return;

            SoundManager.PlaySFX(WorldConsts.AudioConsts.Reward);
            StartCoroutine(AnimateAddCaps());
        }

        private IEnumerator AnimateAddCaps()
        {
            TweenScale.Begin(CurrentBalanceLabel.gameObject, 0.5f, new Vector3(1.3f, 1.3f, 1.3f));
            yield return new WaitForSeconds(0.6f);
            TweenScale.Begin(CurrentBalanceLabel.gameObject, 0.5f, new Vector3(1.0f, 1.0f, 1.0f));

            yield break;
        }

        public void NoManyAnimate()
        {
            StartCoroutine(AnimateAddCaps());
        }

        private void AddShopItems()
        {
            foreach (IapGoodItem item in GameManager.IapManager.GoodItems)
            {
                var shopItem = NGUITools.AddChild(ShopItemsGrid.gameObject, ShopItemPrefab).GetComponent<ShopItem>();
                shopItem.Init(GameManager, item, this);
                _shopItems.Add(shopItem);
            }
            ShopItemsGrid.Reposition();
        }

        private void AddShopIapItems()
        {
            IapItem_1.Init(GameManager, IapStoreManager.StoreController.products.WithID(IapStoreManager.BUY_1000_DOLLARS_ID));
            IapItem_2.Init(GameManager, IapStoreManager.StoreController.products.WithID(IapStoreManager.BUY_5000_DOLLARS_ID));

            if(GameManager.IapManager.IsBuyFirst30000)
                IapItem_3.Init(GameManager, IapStoreManager.StoreController.products.WithID(IapStoreManager.BUY_30000_DOLLARS_ID));
            else
                IapItem_3.Init(GameManager, IapStoreManager.StoreController.products.WithID(IapStoreManager.BUY_30000_DOLLARS_ID), IapStoreManager.StoreController.products.WithID(IapStoreManager.BUY_30000_DOLLARS_FIRST_ID), true);
        }

        public override void Hide()
        {
            foreach (var shopItem in _shopItems)
            {
                if (shopItem.CloneAnimateItem != null)
                {
                    shopItem.StopAllCoroutines();
                    Destroy(shopItem.CloneAnimateItem);
                }
            }

            base.Hide();
        }

        void OnDestroy()
        {
            CurrencyManager.OnChangeCurrency -= UpdateBalance;
            GameManager.IapManager.OnBuyCurrency -= CurrencyByued;
        }
    }
}
