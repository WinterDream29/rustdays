using System.Collections.Generic;
using Assets.Scripts.Models;
using Assets.Scripts.UI.Interactive;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Controllers.UsableObjects
{
    public class GardenBedInteractive : UsableObject, IInventoryThing
    {
        public List<Transform> Places;

        private GardenBedPanel _panel;
        private InventoryBase _inventory;
        private bool _slotInited;

        private GameManager _gameManager;
        private bool _isUsing;
        private GardenBedState _currentState = GardenBedState.Available;
        private HolderObject _currentItem;
        private List<GameObject> _placedItems = new List<GameObject>();
        private int _maxSeeds = 10;
        private bool _itemsInitialized;
        private int _currentStage;

        protected override void Init()
        {
            base.Init();

            if (!_slotInited)
            {
                _inventory = new InventoryBase();
                _inventory.Init(1);
            }

            _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            StartCoroutine(RefreshProgress());
        }

        public override void Use(GameManager gameManager)
        {
            if (gameManager.DisplayManager.CurrentInteractPanel != null)
                return;

            base.Use(gameManager);

            if (_panel == null)
            {
                _panel = NGUITools.AddChild(gameManager.UiRoot.gameObject, InteractPanelPrefab).GetComponent<GardenBedPanel>();
                _panel.Init(GameManager, _inventory, OnPlantAction, OnGetHarvestAction);
                _panel.Hide();
            }
            if (!_panel.IsShowing)
                StartCoroutine(_panel.ShowDelay(0.2f, _currentItem, _currentState));
        }

        private IEnumerator RefreshProgress()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);

                if (_currentState == GardenBedState.InProgress)
                {
                    if(_currentItem.CurrentDurability > 1)
                        _currentItem.ChangeDurability(1);
                    UpdateItemsPrefabs();

                    if (_currentItem.CurrentDurability <= _currentItem.Item.GardenTimeStage3)
                    {
                        _currentState = GardenBedState.Finished;
                        if (_panel != null)
                        {
                            _panel.CurrentState = GardenBedState.Finished;
                            _panel.UpdateView();
                        }
                    }
                }

                if (_currentState == GardenBedState.Finished && _currentItem.CurrentDurability > 1)
                {
                    _currentItem.ChangeDurability(1);
                    UpdateItemsPrefabs();
                }
            }
        }

        private IEnumerator UpdateItemsPrefabsDelay()
        {
            bool stop = false;
            while (!stop)
            {
                if (_gameManager == null)
                    yield return new WaitForSeconds(0.1f);
                else
                {
                    UpdateItemsPrefabs();
                    stop = true;
                }
            }
            yield break;
        }

        private void InstatiateItems(string prefabPath)
        {
            //if (_itemsInitialized)
            //    return;

            RemoveItemPrefabs();

            for (int i = 0; i < Places.Count; i++)
            {
                if (_placedItems.Count <= i)
                {
                    var prefab = Resources.Load<GameObject>(prefabPath);
                    var go = Instantiate(prefab);
                    go.transform.parent = Places[i];
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localRotation = Quaternion.identity;
                    _placedItems.Add(go);
                }
            }

            _itemsInitialized = true;
        }

        private void UpdateItemsPrefabs()
        {
            if (_currentItem == null || _currentItem.Item == null)
                return;

            //var durability = (int)_currentItem.Item.Durability;
            var curDurability = (int)_currentItem.CurrentDurability;
            //var scaleFactor = 1 - (float)curDurability / durability;

            if (curDurability <= 1)
            {
                if (_currentStage != 4)
                {
                    InstatiateItems(_currentItem.Item.GardenPlacedPrefabWithered);
                    _currentStage = 4;
                    if (_panel != null)
                        _panel.UpdateView();
                }
            }
            else if (_currentItem.CurrentDurability <= _currentItem.Item.GardenTimeStage1 && _currentStage != 1 && _currentStage != 2 && _currentStage != 3 && _currentStage != 4)
            {
                InstatiateItems(_currentItem.Item.GardenPlacedPrefabStage1);
                _currentStage = 1;
            }
            else if (_currentItem.CurrentDurability <= _currentItem.Item.GardenTimeStage2 && _currentStage != 2 && _currentStage != 3 && _currentStage != 4)
            {
                InstatiateItems(_currentItem.Item.GardenPlacedPrefabStage2);
                _currentStage = 2;
            }
            else if (_currentItem.CurrentDurability <= _currentItem.Item.GardenTimeStage3 && _currentStage != 3 && _currentStage != 4)
            {
                InstatiateItems(_currentItem.Item.GardenPlacedPrefabStage3);
                _currentStage = 3;
            }
            //foreach (var placedItem in _placedItems)
            //{
            //    var durability = (int)_currentItem.Item.Durability;
            //    var curDurability = (int)_currentItem.CurrentDurability;

            //    var scaleFactor = 1 - (float)curDurability / durability;
            //    placedItem.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            //}
        }

        private void RemoveItemPrefabs()
        {
            foreach (var placedItem in _placedItems)
                Destroy(placedItem);

            _placedItems.Clear();
        }

        private void SetCurretnItem(HolderObject itemHolder)
        {
            _currentItem = itemHolder;
            _currentState = GardenBedState.InProgress;
        }

        private void GeHarvest(HolderObject itemHolder)
        {
            _currentState = GardenBedState.Available;
            var gardenResult = _currentItem.Item.GardenResult;
            if (_currentItem.CurrentDurability <= 1)
                gardenResult = _currentItem.Item.GardenWitheredResult;

            var result = new HolderObject(gardenResult.Key.GetType(), gardenResult.Value * _maxSeeds);
            if(!_gameManager.PlayerModel.Inventory.AddItem(result))
                GameManager.PlacementItemsController.DropItemToGround(GameManager, result);

            _inventory.RemoveItem(_currentItem);
            _currentItem = null;
            RemoveItemPrefabs();
            _itemsInitialized = false;
            _currentStage = 0;
        }

        private void OnPlantAction(HolderObject itemHolder)
        {
            SetCurretnItem(itemHolder);
        }

        private void OnGetHarvestAction(HolderObject itemHolder)
        {
            GeHarvest(itemHolder);
        }

        public bool IsUsing()
        {
            _isUsing = false;
            if (_panel.IsShowing)
                _isUsing = true;
            return _isUsing;
        }

        public List<InventoryBase> GetInventoryList()
        {
            if (_currentItem != null)
                _inventory.AddItem(HolderObjectFactory.GetItem(_currentItem.Item.GetType(), _currentItem.Amount, _currentItem.CurrentDurability));

            var list = new List<InventoryBase> {_inventory};
            return list;
        }

        public void SetInventoryList(List<InventoryBase> inventoryList)
        {
            if (inventoryList != null && inventoryList.Count > 0)
            {
                _inventory = inventoryList[0];
                foreach (var inventorySlot in _inventory.Slots)
                {
                    if (inventorySlot != null && inventorySlot.Item != null)
                    {
                        _currentItem = inventorySlot;
                        if (_currentItem.CurrentDurability > 0)
                            _currentState = GardenBedState.InProgress;
                        else
                        {
                            _currentState = GardenBedState.Finished;
                            if (_panel != null)
                                _panel.CurrentState = GardenBedState.Finished;
                            StartCoroutine(UpdateItemsPrefabsDelay());
                        }
                    }
                }
            }

            _slotInited = true;
        }
    }
}
