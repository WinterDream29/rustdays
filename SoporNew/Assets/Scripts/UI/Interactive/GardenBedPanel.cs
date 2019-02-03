using Assets.Scripts.Models;
using Assets.Scripts.Models.Seedlings;
using Assets.Scripts.Ui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Interactive
{
    public enum GardenBedState
    {
        Available,
        InProgress,
        Finished
    }
    public class GardenBedPanel : View
    {
        public GameObject SeedItemPrefab;
        public UIGrid SeedsGrid;
        public GameObject PlantButton;
        public GameObject HarvestButton;
        public GameObject CloseButton;
        public GameObject PlacedItemObject;
        public UISprite PlacedItemSprite;
        public UILabel PlacedObjectName;
        public UILabel PlacedObjectAmount;
        public UILabel PlacedObjectProgressText;
        public UISprite PlacedObjectProgressSprite;
        public UILabel RequarementAmountLabel;
        public UILabel DescriptionLabel;
        public UISprite HarvestItemSprite;
        public UILabel HarvestItemAmount;
        public UILabel HarvestItemName;
        public UILabel RavnoLabel;
        public InventoryBase Inventory { get; private set; }
        public GardenBedState CurrentState { get; set; }

        private List<GardenBedPanelItem> _seeds = new List<GardenBedPanelItem>();
        private HolderObject _currentItem;
        private int _maxSeeds = 10;
        private Action<HolderObject> _onPlantAction;
        private Action<HolderObject> _onGetHarvestAction;

        public void Init(GameManager gameManager, InventoryBase inventory, Action<HolderObject> onPlantAction, Action<HolderObject> onGetHarvestAction)
        {
            base.Init(gameManager);

            CurrentState = GardenBedState.Available;
            Inventory = inventory;
            _onPlantAction = onPlantAction;
            _onGetHarvestAction = onGetHarvestAction;

            RequarementAmountLabel.text = "(" + Localization.Get("required_amount") + ": " + _maxSeeds + ")";

            UIEventListener.Get(PlacedItemObject).onClick += OnPlacedItemClick;
            UIEventListener.Get(PlantButton).onClick += OnPlantButtonClick;
            UIEventListener.Get(HarvestButton).onClick += OnHarvestButtonClick;
            UIEventListener.Get(CloseButton).onClick += OnCloseClick;
        }

        public IEnumerator ShowDelay(float delayTime, HolderObject currentItem, GardenBedState currentState)
        {
            IsShowing = true;
            yield return new WaitForSeconds(delayTime);
            gameObject.SetActive(true);

            _currentItem = currentItem;
            CurrentState = currentState;
            InitItems();
            UpdateView();
        }

        private void InitItems()
        {
            foreach (var gardenBedPanelItem in _seeds)
                Destroy(gardenBedPanelItem.gameObject);
            _seeds.Clear();

            foreach (var slot in GameManager.PlayerModel.Inventory.Slots)
            {
                if (slot == null || slot.Item == null)
                    continue;

                if (slot.Item is Seedling)
                {
                    var seed = NGUITools.AddChild(SeedsGrid.gameObject, SeedItemPrefab).GetComponent<GardenBedPanelItem>();
                    seed.Set(slot, OnItemClick);
                    _seeds.Add(seed);
                }
            }
            SeedsGrid.Reposition();
        }

        private void SetCurrentItem(HolderObject itemHolder)
        {
            _currentItem = itemHolder;

            RavnoLabel.enabled = CurrentState == GardenBedState.Available;

            PlacedObjectProgressText.enabled = CurrentState == GardenBedState.InProgress;
            PlacedObjectProgressSprite.enabled = CurrentState == GardenBedState.InProgress;
            PlacedObjectName.enabled = _currentItem != null && CurrentState == GardenBedState.Available;
            PlacedItemSprite.enabled = _currentItem != null && CurrentState == GardenBedState.Available;
            PlacedObjectAmount.enabled = _currentItem != null && CurrentState == GardenBedState.Available;

            HarvestItemSprite.enabled = _currentItem != null;
            HarvestItemAmount.enabled = _currentItem != null;
            HarvestItemName.enabled = _currentItem != null;

            HarvestButton.SetActive(CurrentState == GardenBedState.Finished);
            PlantButton.SetActive(CurrentState == GardenBedState.Available);

            if (_currentItem != null)
            {
                PlacedObjectName.text = Localization.Get(_currentItem.Item.LocalizationName);
                PlacedItemSprite.spriteName = _currentItem.Item.IconName;
                PlacedObjectAmount.text = _maxSeeds.ToString();

                var resultItem = _currentItem.Item.GardenResult;
                if (_currentItem.CurrentDurability <= 1)
                    resultItem = _currentItem.Item.GardenWitheredResult;

                HarvestItemName.text = Localization.Get(resultItem.Key.LocalizationName);
                HarvestItemAmount.text = (resultItem.Value * _maxSeeds).ToString();
                HarvestItemSprite.spriteName = resultItem.Key.IconName;
            }
        }

        private void RemoveCurrentItem()
        {
            if (CurrentState == GardenBedState.Available)
            {
                if (_currentItem != null)
                {
                    bool updated = false;
                    foreach (var gardenBedItem in _seeds)
                    {
                        if (gardenBedItem.ItemHolder.Item.GetType() == _currentItem.Item.GetType())
                        {
                            gardenBedItem.UpdateView();
                            updated = true;
                        }
                    }
                    if (!updated)
                    {
                        var seed =
                            NGUITools.AddChild(SeedsGrid.gameObject, SeedItemPrefab).GetComponent<GardenBedPanelItem>();
                        seed.Set(_currentItem, OnItemClick);
                        _seeds.Add(seed);
                    }

                    _currentItem = null;
                    UpdateView();
                    SeedsGrid.Reposition();
                }
            }
        }

        private void PlantItem()
        {
            if (_currentItem == null)
            {
                StartCoroutine(TweenPinPong(DescriptionLabel.gameObject, 0.5f, 1.1f));
                return;
            }

            if (_currentItem.Amount < _maxSeeds)
            {
                StartCoroutine(TweenPinPong(RequarementAmountLabel.gameObject, 0.5f, 1.1f));
                return;
            }

            var itemType = _currentItem.Item.GetType();

            if (_currentItem.Amount <= _maxSeeds)
                GameManager.PlayerModel.Inventory.RemoveItem(_currentItem);
            else
                _currentItem.ChangeAmount(_maxSeeds);

            _currentItem = HolderObjectFactory.GetItem(itemType, _maxSeeds);

            CurrentState = GardenBedState.InProgress;
            UpdateView();

            if (_onPlantAction != null)
                _onPlantAction(_currentItem);
        }

        public override void Hide()
        {
            foreach (var gardenBedItem in _seeds)
                NGUITools.Destroy(gardenBedItem.gameObject);

            _seeds.Clear();

            base.Hide();
        }

        public override void UpdateView()
        {
            base.UpdateView();

            SetCurrentItem(_currentItem);
        }

        private IEnumerator TweenPinPong(GameObject go, float duration, float scaleFactor)
        {
            TweenScale.Begin(go, duration, new Vector3(scaleFactor, scaleFactor));
            yield return new WaitForSeconds(duration);
            TweenScale.Begin(go, duration, new Vector3(1f, 1f));
        }

        private void OnItemClick(GardenBedPanelItem itemView, HolderObject itemHolder)
        {
            if (CurrentState != GardenBedState.Available)
                return;

            if (itemHolder.Amount < _maxSeeds)
            {
                StartCoroutine(TweenPinPong(RequarementAmountLabel.gameObject, 0.5f, 1.1f));
                return;
            }

            if (itemHolder.Amount == _maxSeeds)
            {
                NGUITools.Destroy(itemView.gameObject);
                _seeds.Remove(itemView);
            }
            else
            {
                itemView.AmountLabel.text = (itemHolder.Amount - _maxSeeds).ToString();
            }

            SeedsGrid.Reposition();
            SetCurrentItem(itemHolder);
        }

        private void OnPlacedItemClick(GameObject go)
        {
            RemoveCurrentItem();
        }

        void Update()
        {
            if (CurrentState == GardenBedState.InProgress && _currentItem != null)
            {
                PlacedObjectProgressText.text = GetTime((int)(_currentItem.CurrentDurability - _currentItem.Item.GardenTimeStage3));
                var dur = (int)(_currentItem.Item.Durability - _currentItem.Item.GardenTimeStage3);
                var curDur = (int)(_currentItem.CurrentDurability - _currentItem.Item.GardenTimeStage3);
                PlacedObjectProgressSprite.fillAmount = ((float)curDur / dur);
            }
        }

        private void OnHarvestButtonClick(GameObject go)
        {
            if (_currentItem == null)
                return;

            _currentItem = null;
            CurrentState = GardenBedState.Available;
            UpdateView();

            if (_onGetHarvestAction != null)
                _onGetHarvestAction(_currentItem);
        }

        private void OnPlantButtonClick(GameObject go)
        {
            PlantItem();
        }
        public static string GetTime(int seconds)
        {
            if (seconds / 3600 > 0)
                return string.Format("{0:00}:{1:00}:{2:00}", seconds / 3600, (seconds / 60) % 60, seconds % 60);
            if ((seconds / 60) % 60 > 0)
                return string.Format("{0:00}:{1:00}", (seconds / 60) % 60, seconds % 60);
            return string.Format("{0:00}", seconds % 60);
        }

        private void OnCloseClick(GameObject go)
        {
            Hide();
        }
    }
}
