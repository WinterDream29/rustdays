using System;
using System.Collections.Generic;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.UI.Interactive
{
    public class CookingPanel : InteractView
    {
        public GameObject ItemPrefab;
        public UIGrid SourceSlotsGrid;
        public UIGrid DestionationSlotsGrid;
        public GameObject FireButton;
        public UISprite ObjectIcon;

        public Action<List<UiSlot>, List<UiSlot>> OnSlotsValueChanged;
        public Action<bool> OnFire;

        private List<UiSlot> _destinationSlots;
        private bool _burning;
        private ItemConverterType _typeConverter;

        public void Init(GameManager gameManager, InventoryBase sourceSlots, InventoryBase destinationSlots, bool burning, ItemConverterType typeConverter)
        {
            base.Init(gameManager);

            _burning = burning;
            _typeConverter = typeConverter;
            SetIcon();

            Slots = new List<UiSlot>();
            _destinationSlots = new List<UiSlot>();

            int i = 0;
            foreach (var sourceSlot in sourceSlots.Slots)
            {
                var slot = NGUITools.AddChild(SourceSlotsGrid.gameObject, ItemPrefab).GetComponent<UiSlot>();
                slot.Init(GameManager, sourceSlot, i);
                slot.OnSlotClickAction += OnInventorySlotClick;
                Slots.Add(slot);
                i++;
            }
            i = 0;
            foreach (var destSlot in destinationSlots.Slots)
            {
                var slot = NGUITools.AddChild(DestionationSlotsGrid.gameObject, ItemPrefab).GetComponent<UiSlot>();
                slot.Init(GameManager, destSlot, i);
                _destinationSlots.Add(slot);
                i++;
            }

            foreach (var sourceSlot in Slots)
                sourceSlot.OnManualValueChanged += OnSlotValueChanged;
            foreach (var destSlot in _destinationSlots)
                destSlot.OnManualValueChanged += OnSlotValueChanged;

            SourceSlotsGrid.Reposition();
            DestionationSlotsGrid.Reposition();

            UIEventListener.Get(FireButton).onClick += OnFireClick;
        }

        private void OnInventorySlotClick(UiSlot uiSlot)
        {
            GameManager.Player.OnInventorySlotClick(uiSlot);
        }

        private void SetIcon()
        {
            switch (_typeConverter)
            {
                case ItemConverterType.Campfire:
                    ObjectIcon.spriteName = "camp_fire_icon";
                    break;
                case ItemConverterType.Furnace:
                    ObjectIcon.spriteName = "furnace_icon";
                    break;
            }
        }

        public void UpdateView(InventoryBase sourceSlots, InventoryBase destinationSlots)
        {
            if (sourceSlots != null)
            {
                for (int i = 0; i < Slots.Count; i++)
                    Slots[i].SetData(GameManager, sourceSlots.Slots[i]);
            }
            if (destinationSlots != null)
            {
                for (int i = 0; i < _destinationSlots.Count; i++)
                    _destinationSlots[i].SetData(GameManager, destinationSlots.Slots[i]);
            }
        }

        public override void ChageAmount(UiSlot slot, int amount)
        {
            Slots[slot.SlotId].ChangeAmount(amount);
            OnSlotValueChanged(slot);
        }

        public override void SetItem(UiSlot slot, HolderObject itemModel)
        {
            OnSlotValueChanged(slot);
        }

        private void OnFireClick(GameObject go)
        {
            _burning = !_burning;

            if (OnFire != null)
                OnFire(_burning);
        }

        private void OnSlotValueChanged(UiSlot uiSlot)
        {
            if (OnSlotsValueChanged != null)
                OnSlotsValueChanged(Slots, _destinationSlots);
        }

        public override void Show()
        {
            GameManager.Player.MainHud.InventoryPanel.Show();
            base.Show();
        }

        public override System.Collections.IEnumerator ShowDelay(float delayTime)
        {
            IsShowing = true;
            yield return new WaitForSeconds(delayTime);
            Show();
        }

        public override void Hide()
        {
            //foreach (var sourceSlot in Slots)
            //{
            //    sourceSlot.OnManualValueChanged -= OnSlotValueChanged;
            //}
            //Slots.Clear();
            //Slots = null;
            //foreach (var sourceSlot in _destinationSlots)
            //{
            //    sourceSlot.OnManualValueChanged -= OnSlotValueChanged;
            //}
            //_destinationSlots.Clear();
            //_destinationSlots = null;
            base.Hide();
        }
    }
}
