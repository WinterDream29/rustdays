using System;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Events;
using Assets.Scripts.Ui;
using Assets.Scripts.UI.Craft;
using UnityEngine;
using System.Collections;
using Assets.Scripts.UI.ShopNew;

namespace Assets.Scripts.UI
{
    public enum FlashType
    {
        GetDamage,
        Intoxication
    }

    public class PlayerMainHud : MonoBehaviour
    {
        public AdditionalStatsConstroller AdditionalStats;
        public UISprite HealthProgressSprite;
        public UISprite ThirstProgressSprite;
        public UISprite HungerProgressSprite;
        public UISprite EnergyProgressSprite;
        public InventoryView InventoryPanel;
        public CraftView CraftPanel;
        public NewShopView ShopPanel;
        public GameObject InventoryButton;
        public GameObject CraftButton;
        public GameObject AttackButton;
        public UISprite AttackButtonIcon;
        public GameObject GetItemButton;
        public UISprite GetItemButtonIcon;
        public UISprite GetItemButtonIcon2;
        public Color GetItemButtonIconEnableColor;
        public Color GetItemButtonIconDisableColor;
        public GameObject JumpButton;
        public GameObject RunButton;
        public GameObject RemovePlaceItemButton;
        public GameObject UpFoundationButton;
        public GameObject DownFoundationButton;
        public GameObject UseButton;
        public BoxCollider LookPadCollider;
        public vp_UITouchLook LookPad;
        public BoxCollider Controller;
        public GettingResourceView GettingResourceView;
        public GameObject FlashPanel;
        public UISprite Flash;
        public Color GetDamageFlashColor;
        public Color IntoxicationFlashColor;
        public GameObject OptionsButton;
        public View OptionsDialog;
        public GameObject AutoAimButton;
        public vp_UICrosshair Crosshair;
        public UISprite AutoAimIcon;
        public HudText HudText;
        public StarterPackController StarterPackController;
        public GameObject MainObject;
        public GameObject RightButtonsPlacer;
        public GameObject LeftButtonsPlacer;
        public UILabel CurrencyLabel;
        public GameObject CurrencyObject;
        public GameObject ZoomButton;
        public UILabel AmmoLabel;
        public GameObject SniperZoom;

        public TweenColor HealthColor;
        public TweenColor ThirstColor;
        public TweenColor HungerColor;
        public TweenColor SleepColor;
        public Color DefaultStateColor;

        private bool _healthTweened;
        private bool _thirstTweened;
        private bool _hungerTweened;
        private bool _sleepTweened;

        public bool _getItemButtonIsActive;

        public Action<UiSlot> OnInventorySlotClickAction { get; set; }
        public Action<UiSlot> OnQuickSlotClickAction { get; set; }
        public Action<UiSlot> OnEquipSlotClickAction { get; set; }

        private GameManager _gameManager;
        private SimpleEvents _simpleEvents = new SimpleEvents();

        void Start()
        {
            _simpleEvents.Attach(Player.PLAYER_UPDATE_STATS, OnStatsUpdate);
            _simpleEvents.Attach(Player.PLAYER_GET_DAMAGE, OnGetDamage);
            InventoryPanel.OnSlotClickAction += OnInventorySlotClick;
            InventoryPanel.QuickSlotsPanel.OnSlotClickAction += OnQuickSlotClick;
            InventoryPanel.EquipPanel.OnSlotClickAction += OnEquipSlotClick;

            SetAutoAim(false);
            ZoomButton.SetActive(false);
            AmmoLabel.enabled = false;
            SniperZoom.SetActive(false);

            UIEventListener.Get(InventoryButton).onClick += OnInventoryButtonClick;
            UIEventListener.Get(CraftButton).onClick += OnCraftButtonClick;
            UIEventListener.Get(OptionsButton).onClick += OnOptionButtonClick;
            UIEventListener.Get(AutoAimButton).onClick += OnAutoAimClick;
            UIEventListener.Get(CurrencyObject).onClick += OnShowShop;

            CurrencyManager.OnChangeCurrency += UpdateBalance;

            _getItemButtonIsActive = true;
        }

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;

            RemovePlaceItemButton.SetActive(false);
            GettingResourceView.Init(_gameManager);

            Flash.color = new Color(1, 1, 1, 0);
            FlashPanel.SetActive(false);

            OptionsDialog.Init(_gameManager);
            OptionsDialog.Hide();

            UpdateStats();
            UpdateBalance();

            InventoryPanel.Init(_gameManager);
            InventoryPanel.Hide();
            CraftPanel.Init(_gameManager);
            CraftPanel.Hide();
            ShopPanel.Init(_gameManager);
            ShopPanel.Hide();

            AdditionalStats.Init(_gameManager);
            StarterPackController.Init(_gameManager);
        }

        private void OnShowShop(GameObject go)
        {
            ShopPanel.Show(NewShopCategory.Gold);
        }

        private void OnOptionButtonClick(GameObject go)
        {
            OptionsDialog.Show();
        }

        private void OnAutoAimClick(GameObject go)
        {
            SetAutoAim(!Crosshair.AssistedTargeting);
        }

        private void SetAutoAim(bool isActive)
        {
            Crosshair.AssistedTargeting = isActive;
            AutoAimIcon.color = isActive ? Color.white : Color.black;
        }

        private void UpdateBalance()
        {
            CurrencyLabel.text = CurrencyManager.CurrentCurrency.ToString();
        }

        public void UpdateAmmo(int curAmount, int maxAmount)
        {
            AmmoLabel.text = curAmount + "/" + maxAmount;
        }

        private void OnInventorySlotClick(UiSlot uiSlot)
        {
            if (OnInventorySlotClickAction != null)
                OnInventorySlotClickAction(uiSlot);
        }

        private void OnQuickSlotClick(UiSlot uiSlot)
        {
            if (OnQuickSlotClickAction != null)
                OnQuickSlotClickAction(uiSlot);
        }

        private void OnEquipSlotClick(UiSlot uiSlot)
        {
            if (OnEquipSlotClickAction != null)
                OnEquipSlotClickAction(uiSlot);
        }

        private void UpdateStats()
        {
            HealthProgressSprite.fillAmount = _gameManager.PlayerModel.Health / 100f;
            ThirstProgressSprite.fillAmount = _gameManager.PlayerModel.Thirst / 100f;
            HungerProgressSprite.fillAmount = _gameManager.PlayerModel.Hunger / 100f;
            EnergyProgressSprite.fillAmount = _gameManager.PlayerModel.Energy / 100f;

            AdditionalStats.UpdateStats();

            CheckHudSmall();

            SetGetItemButton();
        }

        private void OnInventoryButtonClick(GameObject go)
        {
            if(InventoryPanel.IsShowing)
            {
                InventoryPanel.Hide();
                SoundManager.PlaySFX(WorldConsts.AudioConsts.CloseInventory);
            }
            else
            {
                InventoryPanel.Show();
                SoundManager.PlaySFX(WorldConsts.AudioConsts.OpenInventory);
            }
        }

        private void OnCraftButtonClick(GameObject go)
        {
            if (CraftPanel.IsShowing)
            {
                CraftPanel.Hide();
                SoundManager.PlaySFX(WorldConsts.AudioConsts.CloseCraft);
            }
            else
            {
                InventoryPanel.Hide();
                CraftPanel.Show();
                SoundManager.PlaySFX(WorldConsts.AudioConsts.OpenCraft);
            }
        }

        private void OnStatsUpdate(object o)
        {
            UpdateStats();
            InventoryPanel.EquipPanel.UpdateItemsDurability();
        }

        private void OnSaveGameClick(GameObject go)
        {
            ProgressManager.SaveProgress(_gameManager);
        }

        private void OnDeleteProgressGameClick(GameObject go)
        {
            ProgressManager.DeleteProgress();
        }

        public void ShowHudText(string text, HudTextColor color = HudTextColor.White, float delay = 1.0f)
        {
            HudText.Show(text, color, delay);
        }

        private void SetGetItemButton()
        {
            if (_gameManager.Player._nearWorldItem != null)
            {
                if (!_getItemButtonIsActive)
                {
                    _getItemButtonIsActive = true;
                    GetItemButtonIcon.color = GetItemButtonIconEnableColor;
                    GetItemButtonIcon2.color = GetItemButtonIconEnableColor;
                }
            }
            else
            {
                if (_getItemButtonIsActive)
                {
                    _getItemButtonIsActive = false;
                    GetItemButtonIcon.color = GetItemButtonIconDisableColor;
                    GetItemButtonIcon2.color = GetItemButtonIconDisableColor;
                }
            }
        }

        public void SetActiveButtons(bool active)
        {
            AttackButton.SetActive(active);
            //UseButton.SetActive(active);
            //JumpButton.SetActive(active);
            RunButton.SetActive(active);
            //ZoomButton.SetActive(active);
        }

        public void SetActiveControls(bool active)
        {
            LookPadCollider.enabled = active;
            Controller.enabled = active;
        }

        public void ShowAddedResource(string spriteName, int amount, string itemName)
        {
            GettingResourceView.ShowItem(spriteName, amount, itemName);
        }

        private void OnGetDamage(object obj)
        {
            StartCoroutine(ShowFlash(FlashType.GetDamage));
        }

        public IEnumerator ShowFlash(FlashType type)
        {
            FlashPanel.SetActive(true);
            switch (type)
            {
                case FlashType.GetDamage:
                    Flash.color = new Color(GetDamageFlashColor.r, GetDamageFlashColor.g, GetDamageFlashColor.b, 0);
                    break;
                case FlashType.Intoxication:
                    Flash.color = new Color(IntoxicationFlashColor.r, IntoxicationFlashColor.g, IntoxicationFlashColor.b, 0);
                    break;
            }

            TweenAlpha.Begin(Flash.gameObject, 0.3f, 0.6f);
            yield return new WaitForSeconds(0.4f);
            TweenAlpha.Begin(Flash.gameObject, 0.3f, 0.0f);
            yield return new WaitForSeconds(0.3f);

            FlashPanel.SetActive(false);
        }

        private void CheckHudSmall()
        {
            if (ThirstProgressSprite.fillAmount < 0.2f && !_thirstTweened)
            {
                _thirstTweened = true;
                ThirstColor.enabled = true;
            }
            else if (ThirstProgressSprite.fillAmount >= 0.2f && _thirstTweened)
            {
                _thirstTweened = false;
                ThirstColor.value = DefaultStateColor;
                ThirstColor.enabled = false;
            }

            if (HungerProgressSprite.fillAmount < 0.2f && !_hungerTweened)
            {
                _hungerTweened = true;
                HungerColor.enabled = true;
            }
            else if (HungerProgressSprite.fillAmount >= 0.2f && _hungerTweened)
            {
                _hungerTweened = false;
                HungerColor.value = DefaultStateColor;
                HungerColor.enabled = false;
            }

            if (EnergyProgressSprite.fillAmount < 0.2f && !_sleepTweened)
            {
                _sleepTweened = true;
                SleepColor.enabled = true;
            }
            else if (EnergyProgressSprite.fillAmount >= 0.2f && _sleepTweened)
            {
                _sleepTweened = false;
                SleepColor.value = DefaultStateColor;
                SleepColor.enabled = false;
            }

            if ((ThirstProgressSprite.fillAmount <= 0.0f || HungerProgressSprite.fillAmount <= 0.0f) && !_healthTweened)
            {
                _healthTweened = true;
                HealthColor.enabled = true;
            }
            else if (ThirstProgressSprite.fillAmount > 0.0f && HungerProgressSprite.fillAmount > 0.0f && _healthTweened)
            {
                _healthTweened = false;
                HealthColor.value = DefaultStateColor;
                HealthColor.enabled = false;
            }
        }

        public void OnUnderWater(object under)
        {
            AdditionalStats.SetBreath((bool)under);
        }

        void OnDestroy()
        {
            _simpleEvents.Detach(Player.PLAYER_UPDATE_STATS, OnStatsUpdate);
            _simpleEvents.Detach(Player.PLAYER_GET_DAMAGE, OnGetDamage);
            _simpleEvents = null;

            InventoryPanel.QuickSlotsPanel.OnSlotClickAction -= OnQuickSlotClick;
            InventoryPanel.OnSlotClickAction -= OnInventorySlotClick;

            if(CurrencyManager.OnChangeCurrency != null)
                CurrencyManager.OnChangeCurrency -= UpdateBalance;
        }
    }
}