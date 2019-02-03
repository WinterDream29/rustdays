using Assets.Scripts.Models.Food;
using Assets.Scripts.Ui;
using UnityEngine;
using Assets.Scripts.Models;
using Assets.Scripts.UI.Shop;
using UnityEngine.Advertisements;

namespace Assets.Scripts.UI.Dialogs
{
    public class SleepDialog : View
    {
        public GameObject UseCoffeeObject;
        public GameObject SaveExitButton;
        public GameObject UseCoffeeButton;
        public GameObject BuyCoffeeButton;
        public GameObject BuyGoldObject;
        public UIGrid MainGrid;
        public GameObject GoldObject;
        public GameObject GoldObjectCloseCollider;
        public UIGrid GoldGrid;
        public UILabel AmountCoffeeInInventoryLabel;
        public UILabel CoffeePriceLabel;
        public UILabel AmountCoinsLabel;
        public ShopIAPItem IapItem_1;
        public ShopIAPItem IapItem_2;
        public ShopIAPItem IapItem_3;
        public GameObject AdsRefillButton;

        private double _price;
        private IapGoodItem _goodItem;
        private int _amountInInventory;

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);

            AddShopIAPItems();
            GoldObject.SetActive(false);

            UpdateAmountCoffeeInInventory();
            InitBuyCoffee();
            UpdateBalance();

            UIEventListener.Get(SaveExitButton).onClick += OnSaveExitClick;
            UIEventListener.Get(UseCoffeeButton).onClick += OnUseCoffeeClick;
            UIEventListener.Get(BuyCoffeeButton).onClick += OnBuyCoffeeClick;
            UIEventListener.Get(AdsRefillButton).onClick += ShowRewardedAd;
            UIEventListener.Get(GoldObjectCloseCollider).onClick += OnCloseGoldObjecteClick;
            CurrencyManager.OnChangeCurrency += UpdateBalance;
            GameManager.IapManager.OnBuyCurrency += CurrencyByued;
        }

        public override void Show()
        {
            if (GameManager.Player.MainHud.InventoryPanel.IsShowing)
                GameManager.Player.MainHud.InventoryPanel.Hide();
            if (GameManager.Player.MainHud.CraftPanel.IsShowing)
                GameManager.Player.MainHud.CraftPanel.Hide();

            if (GameManager.DisplayManager.CurrentInteractPanel != null)
                GameManager.DisplayManager.CurrentInteractPanel.Hide();

            GameManager.Player.MainHud.SetActiveButtons(false);
            GameManager.Player.MainHud.SetActiveControls(false);

            UpdateAmountCoffeeInInventory();
            UpdateBalance();
            base.Show();
        }

        public override void Hide()
        {
            GameManager.Player.MainHud.SetActiveButtons(true);
            GameManager.Player.MainHud.SetActiveControls(true);
            base.Hide();
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
                    GameManager.PlayerModel.ChangeEnergy(20.0f);
                    break;
                case ShowResult.Skipped:
                    Debug.Log("The ad was skipped before reaching the end.");
                    break;
                case ShowResult.Failed:
                    Debug.LogError("The ad failed to be shown.");
                    break;
            }
        }

        private void UpdateAmountCoffeeInInventory()
        {
            _amountInInventory = GameManager.PlayerModel.Inventory.GetAmount(typeof(Adrenaline));
            AmountCoffeeInInventoryLabel.text = "x" + _amountInInventory;

            MainGrid.Reposition();
        }

        private void InitBuyCoffee()
        {
            foreach(var item in GameManager.IapManager.GoodItems)
            {
                if(item.RewardItemName == typeof(Adrenaline).Name)
                {
                    _goodItem = item;
                    break;
                }
            }

            CoffeePriceLabel.text = _goodItem.Price.ToString();
        }

        private void UpdateBalance()
        {
            AmountCoinsLabel.text = CurrencyManager.CurrentCurrency.ToString();
        }

        private void CurrencyByued(string id)
        {
            if (!IsShowing)
                return;

            SoundManager.PlaySFX(WorldConsts.AudioConsts.Reward);
        }

        private void AddShopIAPItems()
        {
            IapItem_1.Init(GameManager, IapStoreManager.StoreController.products.WithID(IapStoreManager.BUY_1000_DOLLARS_ID));
            IapItem_2.Init(GameManager, IapStoreManager.StoreController.products.WithID(IapStoreManager.BUY_5000_DOLLARS_ID));
            IapItem_3.Init(GameManager, IapStoreManager.StoreController.products.WithID(IapStoreManager.BUY_30000_DOLLARS_ID));
        }

        private void OnSaveExitClick(GameObject go)
        {
            ProgressManager.SaveProgress(GameManager);
            Application.Quit();
        }

        private void OnUseCoffeeClick(GameObject go)
        {
            if (_amountInInventory <= 0)
                return;

            foreach(var slot in GameManager.Player.MainHud.InventoryPanel.Slots)
            {
                if(slot != null && slot.ItemModel != null && slot.ItemModel.Item != null && slot.ItemModel.Item.GetType() == typeof(Adrenaline))
                {
                    slot.ItemModel.Use(GameManager);
                    UpdateAmountCoffeeInInventory();
                    Hide();
                    return;
                }
            }

            foreach (var slot in GameManager.Player.MainHud.InventoryPanel.QuickSlotsPanel.Slots)
            {
                if (slot != null && slot.ItemModel != null && slot.ItemModel.Item != null && slot.ItemModel.Item.GetType() == typeof(Adrenaline))
                {
                    slot.ItemModel.Use(GameManager);
                    UpdateAmountCoffeeInInventory();
                    Hide();
                    return;
                }
            }
        }

        private void OnBuyCoffeeClick(GameObject go)
        {
            if (CurrencyManager.CurrentCurrency >= _goodItem.Price)
            {
                CurrencyManager.AddCurrency(-_goodItem.Price);
                GameManager.PlayerModel.Inventory.AddItem(HolderObjectFactory.GetItem(typeof(Adrenaline), _goodItem.RewardAmount));
                UpdateBalance();
                UpdateAmountCoffeeInInventory();
            }
            else
            {
                GoldObject.SetActive(true);
            }
        }

        private void OnCloseGoldObjecteClick(GameObject go)
        {
            GoldObject.SetActive(false);
        }

        void Update()
        {
            if (GameManager.PlayerModel.Energy > 5)
                Hide();
        }

        void OnDestroy()
        {
            CurrencyManager.OnChangeCurrency -= UpdateBalance;
            GameManager.IapManager.OnBuyCurrency -= CurrencyByued;
        }
    }
}
