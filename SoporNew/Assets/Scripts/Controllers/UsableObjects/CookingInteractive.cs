using System.Collections.Generic;
using Assets.Scripts.Models;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Interactive;
using UnityEngine;

namespace Assets.Scripts.Controllers.UsableObjects
{
    public class CookingInteractive : UsableObject, IInventoryThing, IConverter
    {
        public ParticleSystem SmokeParticles;
        public GameObject LightGameObject;
        public AudioSource FireSound;
        public ItemConverterType TypeConverter;
        public int SourceSlotsAmount;
        public int DestinationSlotsAmount;

        public bool IsBurning { get; private set; }
        public bool AutoStart { get; set; }

        private InventoryBase _sourceinventory;
        private InventoryBase _destinationInventory;
        private CookingPanel _panel;

        private bool _slotInited;
        private float _currentBurnTime;
        private float _outBurnTime = 10f;
        private int _firedWoods;

        protected override void Init()
        {
            base.Init();

            if (!_slotInited)
            {
                _sourceinventory = new InventoryBase();
                _sourceinventory.Init(SourceSlotsAmount);
                _destinationInventory = new InventoryBase();
                _destinationInventory.Init(DestinationSlotsAmount);

                LightGameObject.SetActive(false);

                _slotInited = true;
            }

            if(AutoStart)
                Fire();
        }

        public override void Use(GameManager gameManager)
        {
            if (gameManager.DisplayManager.CurrentInteractPanel != null)
                return;

            base.Use(gameManager);

            if (_panel == null)
            {
                _panel = NGUITools.AddChild(gameManager.UiRoot.gameObject, InteractPanelPrefab).GetComponent<CookingPanel>();
                _panel.Init(GameManager, _sourceinventory, _destinationInventory, IsBurning, TypeConverter);
                _panel.OnSlotsValueChanged += OnSlotsValueChanged;
                _panel.OnFire += OnFireClick;
                _panel.Hide();
            }

            if (!_panel.IsShowing)
                StartCoroutine(_panel.ShowDelay(0.2f));
        }

        private void OnFireClick(bool isFire)
        {
            if (isFire)
            {
                Fire();
            }
            else
            {
               SnuffOut();
            }
        }

        void Update()
        {
            if (IsBurning)
            {
                _currentBurnTime += Time.deltaTime;

                if (_currentBurnTime >= _outBurnTime)
                {
                    _currentBurnTime = 0.0f;
                    RefreshCooking();
                }
            }
        }

        private void RefreshCooking()
        {
            _firedWoods++;
            foreach (var holderObject in _sourceinventory.Slots)
            {
                if (holderObject != null && holderObject.Item != null && holderObject.Item is WoodResource)
                {
                    _destinationInventory.AddItem(HolderObjectFactory.GetItem(holderObject.Item.CookingResult.Key.GetType(), holderObject.Item.CookingResult.Value));
                    holderObject.ChangeAmount(1);
                }
                else if (holderObject != null &&
                    holderObject.Amount > 0 &&
                    holderObject.Item.CookingResult.Key != null &&
                    holderObject.Item.Converters.Contains(TypeConverter))
                {
                    if (_firedWoods % holderObject.Item.NeedWoodToCook == 0)
                    {
                        _destinationInventory.AddItem(HolderObjectFactory.GetItem(holderObject.Item.CookingResult.Key.GetType(), holderObject.Item.CookingResult.Value));
                        holderObject.ChangeAmount(1);
                    }
                }
            }

            if(_panel != null)
                _panel.UpdateView(_sourceinventory, _destinationInventory);

            if(!HasWood())
                SnuffOut();
        }

        public void Fire()
        {
            if (HasWood())
            {
                IsBurning = true;
                _currentBurnTime = 0.0f;
                SmokeParticles.Play();
                LightGameObject.SetActive(true);
                FireSound.Play();

                GameCenterManager.ProgressAchievement(GameCenterManager.AchEarnerFireId);
                GooglePlayServicesController.Unlock(GPGSIds.achievement_earner_fire);
            }
        }

        private bool HasWood()
        {
            foreach (var holderObject in _sourceinventory.Slots)
                if (holderObject != null && holderObject.Item is WoodResource && holderObject.Amount > 0)
                    return true;

            return false;
        }

        private void SnuffOut()
        {
            IsBurning = false;
            SmokeParticles.Stop();
            LightGameObject.SetActive(false);
            FireSound.Stop();
        }

        private void OnSlotsValueChanged(List<UiSlot> sourceSlots, List<UiSlot> destSlots)
        {
            for (int i = 0; i < _sourceinventory.Slots.Count; i++)
            {
                _sourceinventory.Slots[i] = sourceSlots[i].ItemModel;
                if (_sourceinventory.Slots[i] != null)
                {
                    if (_sourceinventory.Slots[i].OnRemoved != null)
                        _sourceinventory.Slots[i].OnRemoved -= OnSourceHolderObjectRemoved;
                    _sourceinventory.Slots[i].OnRemoved += OnSourceHolderObjectRemoved;
                }
            }

            for (int i = 0; i < _destinationInventory.Slots.Count; i++)
            {
                _destinationInventory.Slots[i] = destSlots[i].ItemModel;
                if (_destinationInventory.Slots[i] != null)
                {
                    if (_destinationInventory.Slots[i].OnRemoved != null)
                        _destinationInventory.Slots[i].OnRemoved -= OnDestHolderObjectRemoved;
                    _destinationInventory.Slots[i].OnRemoved += OnDestHolderObjectRemoved;
                }
            }
        }

        private void OnSourceHolderObjectRemoved(HolderObject holderObject)
        {
            if(holderObject == null)
                return;

            var index = _sourceinventory.Slots.IndexOf(holderObject);
            if (index != -1)
                _sourceinventory.Slots[index] = null;
        }

        private void OnDestHolderObjectRemoved(HolderObject holderObject)
        {
            if (holderObject == null)
                return;

            var index = _destinationInventory.Slots.IndexOf(holderObject);
            if (index != -1)
                _destinationInventory.Slots[index] = null;
        }

        public List<InventoryBase> GetInventoryList()
        {
            return new List<InventoryBase> {_sourceinventory, _destinationInventory};
        }

        public void SetInventoryList(List<InventoryBase> inventoryList)
        {
            if (inventoryList != null && inventoryList.Count > 0)
            {
                if (inventoryList.Count > 0)
                    _sourceinventory = inventoryList[0];
                if (inventoryList.Count > 1)
                    _destinationInventory = inventoryList[1];
            }
            _slotInited = true;
        }

        protected override void Destroyed()
        {

            base.Destroyed();
        }
    }
}
