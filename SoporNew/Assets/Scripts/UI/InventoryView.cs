using System;
using System.Collections.Generic;
using Assets.Scripts.Ui;
using UnityEngine;
using Assets.Scripts.Models.Events;
using Assets.Scripts.Models;

namespace Assets.Scripts.UI
{
    public class InventoryView : View
    {
        public GameObject ItemPrefab;
        public UIGrid SlotsGrid;
        public QuickSlotsView QuickSlotsPanel;
        public EquipView EquipPanel;
        public BoxCollider BackCollider;

        public Action<UiSlot> OnSlotClickAction { get; set; }

        public List<UiSlot> Slots = new List<UiSlot>();

        private SimpleEvents _simpleEvents = new SimpleEvents();

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);
            QuickSlotsPanel.Init(gameManager);
            EquipPanel.Init(gameManager);

            var intentoryModel = GameManager.PlayerModel.Inventory;
            for (int i = 0; i < intentoryModel.MaxSlots; i++)
            {
                var itemModel = GameManager.PlayerModel.Inventory.Slots[i];
                var go = NGUITools.AddChild(SlotsGrid.gameObject, ItemPrefab);
                go.name = "InventorySlot_" + i;
                var slotUi = go.GetComponent<UiSlot>();
                slotUi.Init(GameManager, itemModel, i);
                slotUi.OnSlotClickAction += OnSlotClick;
                Slots.Add(slotUi);
            }
            SlotsGrid.Reposition();

            UIEventListener.Get(BackCollider.gameObject).onClick += OnBackClick;
            _simpleEvents.Attach(Inventory.INVENTORY_ADD_ITEM, OnAddItem);
        }

        public void AddNewSlots(int amount)
        {
            var intentoryModel = GameManager.PlayerModel.Inventory;

            for (int i = intentoryModel.MaxSlots - amount; i < intentoryModel.MaxSlots; i++)
            {
                var itemModel = GameManager.PlayerModel.Inventory.Slots[i];
                var go = NGUITools.AddChild(SlotsGrid.gameObject, ItemPrefab);
                go.name = "InventorySlot_" + i;
                var slotUi = go.GetComponent<UiSlot>();
                slotUi.Init(GameManager, itemModel, i);
                slotUi.OnSlotClickAction += OnSlotClick;
                Slots.Add(slotUi);
            }
        }

        public override void Show()
        {
            base.Show();
            EquipPanel.Show();
            UpdateView();

            GameManager.Player.MainHud.SetActiveControls(false);
            GameManager.Player.MainHud.SetActiveButtons(false);
        }

        public override void Hide()
        {
            GameManager.Player.MainHud.SetActiveControls(true);
            GameManager.Player.MainHud.SetActiveButtons(true);
            EquipPanel.Hide();
            if (GameManager.DisplayManager.CurrentInteractPanel != null)
                GameManager.DisplayManager.CurrentInteractPanel.Hide();

            base.Hide();
        }

        public override void UpdateView()
        {
            base.UpdateView();

            var intentoryModel = GameManager.PlayerModel.Inventory;
            for (int i = 0; i < intentoryModel.MaxSlots; i++)
            {
                var itemModel = GameManager.PlayerModel.Inventory.Slots[i];
                Slots[i].SetData(GameManager, itemModel);
            }
            SlotsGrid.Reposition();

            QuickSlotsPanel.UpdateView();
            EquipPanel.UpdateView();
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

        private void OnBackClick(GameObject go)
        {
            Hide();
        }

        public override void Destroyed()
        {
            base.Destroyed();
            _simpleEvents.Detach(Inventory.INVENTORY_ADD_ITEM, OnAddItem);
        }
    }
}
