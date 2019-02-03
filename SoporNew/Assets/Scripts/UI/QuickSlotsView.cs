using System;
using System.Collections.Generic;
using Assets.Scripts.Ui;
using Assets.Scripts.Models.Events;
using Assets.Scripts.Models;

namespace Assets.Scripts.UI
{
    public class QuickSlotsView : View
    {
        public List<UiSlot> Slots;

        public Action<UiSlot> OnSlotClickAction { get; set; }
        public UiSlot CurrentSlot { get; set; }
        public UiSlot CurrentPlacementSlot { get; set; }

        private SimpleEvents _simpleEvents = new SimpleEvents();

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);

            for (int i = 0; i < WorldConsts.QuickSlotsAmount; i++)
            {
                Slots[i].Init(GameManager, GameManager.PlayerModel.Inventory.QuickSlots[i], i);
                Slots[i].OnSlotClickAction += OnSlotClick;
            }

            _simpleEvents.Attach(Inventory.INVENTORY_ADD_QUICK_SLOT_ITEM, OnAddItem);
        }

        private void OnAddItem(object o)
        {
            UpdateView();
        }

        private void OnSlotClick(UiSlot uiSlot)
        {
            if (OnSlotClickAction != null)
                OnSlotClickAction(uiSlot);
        }

        public override void UpdateView()
        {
            base.UpdateView();

            for (int i = 0; i < WorldConsts.QuickSlotsAmount; i++)
            {
                var model = GameManager.PlayerModel.Inventory.QuickSlots[i];
                Slots[i].SetData(GameManager, model);
            }
        }

        public void LockView()
        {
            
        }

        public override void Destroyed()
        {
            base.Destroyed();
            _simpleEvents.Detach(Inventory.INVENTORY_ADD_QUICK_SLOT_ITEM, OnAddItem);
        }
    }
}
