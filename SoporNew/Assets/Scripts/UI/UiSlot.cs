using System;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Clothes;
using Assets.Scripts.Ui;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public enum SlotType
    {
        EquipCap,
        EquipShirt,
        EquipPants,
        EquipBoots,
        Quick,
        Inventory,
        Interactive
    }

    public class UiSlot : View
    {
        public SlotType SlotType = SlotType.Inventory;
        public GameObject DraggableItemPrefab;
        public UISprite Icon;
        public UILabel Amount;
        public UISprite SelectedSprite;
        public UISprite SelectedUsableSprite;
        public GameObject DurabilityObject;
        public UISprite DurabilityProgressSprite;

        public bool IsSelected { get; set; }
        public bool IsSelectedUsable { get; set; }
        public Action<UiSlot> OnSlotClickAction;
        public Action<UiSlot> OnValueChanged;
        public Action<UiSlot> OnManualValueChanged;

        public HolderObject ItemModel { get; private set; }
        public int SlotId { get; set; }

        private GameObject _dragObject;
        private GameManager _gameManager;

        private float _changeEquipDurabilityTime = 1f;
        private float _currentEquipDurabilityTime = 0f;

        void Start()
        {
            SelectedSprite.enabled = false;
            if (SelectedUsableSprite != null)
                SelectedUsableSprite.enabled = false;

            if(DurabilityObject != null)
            {
                if(ItemModel != null)
                    DurabilityObject.SetActive(ItemModel.CurrentDurability != null && ItemModel.Item.ShowDurability);
                else
                    DurabilityObject.SetActive(false);
            }

            UIEventListener.Get(gameObject).onClick += OnSlotClick;
        }

        public void Init(GameManager gameManager, HolderObject itemModel, int slotId)
        {
            ItemModel = itemModel;
            _gameManager = gameManager;
            SlotId = slotId;
            UpdateView();

            if (ItemModel != null)
            {
                ItemModel.OnRemoved += OnItemModelRemoved;
                ItemModel.OnAmountChanged += OnAmountChanged;
                ItemModel.OnDurabilityChanged += OnDurabilityChanged;
            }
        }

        public void SetData(GameManager gameManager, HolderObject itemModel)
        {
            _gameManager = gameManager;

            if (ItemModel != null)
            {
                if (ItemModel.OnRemoved != null)
                    ItemModel.OnRemoved -= OnItemModelRemoved;
                if (ItemModel.OnAmountChanged != null)
                    ItemModel.OnAmountChanged -= OnAmountChanged;
                if (ItemModel.OnDurabilityChanged != null)
                    ItemModel.OnDurabilityChanged -= OnDurabilityChanged;
            }

            if (itemModel != null && itemModel.Item != null)
                ItemModel = HolderObjectFactory.GetItem(itemModel.Item.GetType().Name, itemModel.Amount, itemModel.CurrentDurability);
            else
                ItemModel = null;

           _gameManager.PlayerModel.SetItem(this, ItemModel);

            if (ItemModel != null)
            {
                ItemModel.OnRemoved += OnItemModelRemoved;
                ItemModel.OnAmountChanged += OnAmountChanged;
                ItemModel.OnDurabilityChanged += OnDurabilityChanged;
            }
            UpdateView();
        }

        private void OnItemModelRemoved(HolderObject itemModel)
        {
            _gameManager.PlayerModel.SetItem(this, null);
            ItemModel = null;
            UpdateView();

            if (OnValueChanged != null)
                OnValueChanged(this);
        }

        private void OnAmountChanged()
        {
            UpdateView();

            if (OnValueChanged != null)
                OnValueChanged(this);
        }

        private void OnDurabilityChanged()
        {
            UpdateView();
        }

        public override void UpdateView()
        {
            base.UpdateView();
            if (ItemModel == null || ItemModel.Item == null)
            {
                Icon.enabled = false;
                Amount.enabled = false;
                if(DurabilityObject != null)
                    DurabilityObject.SetActive(false);
                if(SelectedSprite != null)
                    SelectedSprite.enabled = false;
                if (SelectedUsableSprite != null)
                    SelectedUsableSprite.enabled = false;
                return;
            }

            if (DurabilityObject != null)
                DurabilityObject.SetActive(ItemModel.CurrentDurability != null && ItemModel.Item.ShowDurability);
            Amount.enabled = ItemModel.CurrentDurability == null || !ItemModel.Item.ShowDurability;

            Icon.enabled = true;
            Icon.spriteName = ItemModel.Item.IconName;
            Amount.text = ItemModel.Amount.ToString();
            if(ItemModel.CurrentDurability != null && ItemModel.Item.ShowDurability)
                DurabilityProgressSprite.fillAmount = (float)ItemModel.CurrentDurability / (float)ItemModel.Item.Durability;
        }

        public void SetActiveSlot(bool active)
        {
            IsSelected = active;
            SelectedSprite.enabled = active;
        }

        public void SelectUsable(bool active)
        {
            if (SelectedUsableSprite != null)
                SelectedUsableSprite.enabled = active;

            IsSelectedUsable = active;
        }

        public void SetItem(BaseObject item, int amount = 1)
        {
            ItemModel = new HolderObject(item.GetType(), amount);
        }

        public void ChangeAmount(int amount = 1)
        {
            ItemModel.ChangeAmount(amount);
        }

        public void RemoveItem(BaseObject item, int amount = 1)
        {
            ItemModel = new HolderObject(item.GetType(), amount);
        }

        private void OnSlotClick(GameObject go)
        {
            if (OnManualValueChanged != null)
                OnManualValueChanged(this);

            if (OnSlotClickAction != null)
                OnSlotClickAction(this);
        }

        private void InstantiateDragObject()
        {
            _dragObject = Instantiate(DraggableItemPrefab);
            _dragObject.transform.parent = transform;
            _dragObject.transform.localPosition = Vector3.zero;
            _dragObject.transform.localScale = Vector3.one;
            var dragItemController = _dragObject.GetComponent<UiDraggableSlot>();
            dragItemController.Init(ItemModel);
        }

        void OnDragStart()
        {
            if (ItemModel == null || IsSelectedUsable)
                return;

            SetActiveSlot(false);
            InstantiateDragObject();
        }

        void OnDrag(Vector2 delta)
        {
            if (ItemModel == null || IsSelectedUsable)
                return;

            var pos = _dragObject.transform.localPosition;
            var pixelSizeAdj = _gameManager.DisplayManager.UiRoot.pixelSizeAdjustment;
            pos.x += delta.x * pixelSizeAdj;
            pos.y += delta.y * pixelSizeAdj;
            _dragObject.transform.localPosition = pos;
        }

        void OnDragEnd()
        {
            if (UICamera.hoveredObject != null && UICamera.hoveredObject.tag == "ToDropItem" && !IsSelectedUsable)
            {
                _gameManager.PlacementItemsController.DropItemToGround(_gameManager, ItemModel);
                SetData(_gameManager, null);
                Destroy(_dragObject);
            }

            if (ItemModel == null)
                return;

            Destroy(_dragObject);
        }
        void OnDrop(GameObject go)
        {
            var slot = go.GetComponent<UiSlot>();
            if (slot == null)
                return;

            if(!CanSetEquip(slot))
                return;

            var slotItem = slot.ItemModel;

            if (ItemModel != null && ItemModel.Item != null && slot.ItemModel != null && slot.ItemModel.Item != null 
                && slot.ItemModel.Item.GetType() == ItemModel.Item.GetType()
                && (slot.ItemModel.Item.Durability == null || !slot.ItemModel.Item.ShowDurability))
            {
                ItemModel.ChangeAmount(-slotItem.Amount);
                slot.SetData(_gameManager, null);
            }
            else
            {
                slot.SetData(_gameManager, ItemModel);
                SetData(_gameManager, slotItem);
            }

            if (OnManualValueChanged != null)
                OnManualValueChanged(this);
        }
        public bool CanSetEquip(UiSlot slot)
        {
            if (SlotType == SlotType.EquipCap)
                if (!(slot.ItemModel.Item is Cap))
                    return false;
            if (SlotType == SlotType.EquipShirt)
                if (!(slot.ItemModel.Item is Shirt))
                    return false;
            if (SlotType == SlotType.EquipPants)
                if (!(slot.ItemModel.Item is Pants))
                    return false;
            if (SlotType == SlotType.EquipBoots)
                if (!(slot.ItemModel.Item is Boots))
                    return false;

            return true;
        }

        public void UpdateDurability()
        {
            if (ItemModel != null)
            {
                ItemModel.ChangeDurability(1);
                _currentEquipDurabilityTime = 0f;
            }
        }

        public override void Destroyed()
        {
            if (ItemModel != null)
            {
                if (ItemModel.OnRemoved != null)
                    ItemModel.OnRemoved -= OnItemModelRemoved;
                if (ItemModel.OnAmountChanged != null)
                    ItemModel.OnAmountChanged -= OnAmountChanged;
                if (ItemModel.OnDurabilityChanged != null)
                    ItemModel.OnDurabilityChanged -= OnDurabilityChanged;
            }

            StopAllCoroutines();

            base.Destroyed();
        }
    }
}