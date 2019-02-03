using System;
using System.Collections.Generic;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.UI.Interactive
{
    public class InventoryPanel : InteractView
    {
        public UIGrid SlotsGrid;
        public GameObject ItemPrefab;

        public Action<List<UiSlot>> OnSlotsValueChanged;

        public InventoryBase InventorySlots;

        public virtual void Init(GameManager gameManager, InventoryBase slots)
        {
            base.Init(gameManager);

            InventorySlots = slots;
            Slots = new List<UiSlot>();
            int i = 0;
            foreach (var sourceSlot in InventorySlots.Slots)
            {
                var slot = NGUITools.AddChild(SlotsGrid.gameObject, ItemPrefab).GetComponent<UiSlot>();
                slot.Init(GameManager, sourceSlot, i);
                slot.OnManualValueChanged += OnSlotManualValueChanged;
                slot.OnValueChanged += OnSlotValueChanged;
                slot.OnSlotClickAction += OnInventorySlotClick;
                Slots.Add(slot);
                i++;
            }

            SlotsGrid.Reposition();
        }

        public override void ChageAmount(UiSlot slot, int amount)
        {
            Slots[slot.SlotId].ChangeAmount(amount);
            if (OnSlotsValueChanged != null)
                OnSlotsValueChanged(Slots);
        }

        private void OnInventorySlotClick(UiSlot uiSlot)
        {
            GameManager.Player.OnInventorySlotClick(uiSlot);
        }

        private void OnSlotManualValueChanged(UiSlot uiSlot)
        {
            if (OnSlotsValueChanged != null)
                OnSlotsValueChanged(Slots);
        }

        private void OnSlotValueChanged(UiSlot uiSlot)
        {
            if (OnSlotsValueChanged != null)
                OnSlotsValueChanged(Slots);
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
            //    sourceSlot.OnSlotClickAction -= OnInventorySlotClick;
            //    sourceSlot.OnValueChanged -= OnSlotManualValueChanged;
            //    sourceSlot.OnValueChanged -= OnSlotValueChanged;
            //}
            //Slots.Clear();
            //Slots = null;

            base.Hide();
        }
    }
}
