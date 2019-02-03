using System;
using System.Collections.Generic;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Interactive;

namespace Assets.Scripts.Controllers.UsableObjects
{
    [Serializable]
    public struct StartItem
    {
        public string name;
        public int amount;
    }
    public class InventoryInteractive : UsableObject, IInventoryThing
    {
        public int MaxSlots;
        public List<StartItem> StartItems;

        private InventoryBase _inventory;
        private InventoryPanel _panel;
        private bool _slotInited;

        protected override void Init()
        {
            base.Init();

            if (!_slotInited)
            {
                _inventory = new InventoryBase();
                _inventory.Init(MaxSlots);
                foreach(var item in StartItems)
                {
                    _inventory.AddItem(HolderObjectFactory.GetItem(item.name, item.amount));
                }
            }
        }

        public override void Use(GameManager gameManager)
        {
            if (gameManager.DisplayManager.CurrentInteractPanel != null)
                return;

            base.Use(gameManager);

            if (_panel == null)
            {
                _panel = NGUITools.AddChild(gameManager.UiRoot.gameObject, InteractPanelPrefab).GetComponent<InventoryPanel>();
                _panel.Init(GameManager, _inventory);
                _panel.OnSlotsValueChanged += OnSlotsValueChanged;
                _panel.Hide();
            }
            if (!_panel.IsShowing)
            {
                StartCoroutine(_panel.ShowDelay(0.2f));
            }
        }

        private void OnSlotsValueChanged(List<UiSlot> uiSlots)
        {
            for (int i = 0; i < _inventory.Slots.Count; i++)
                _inventory.Slots[i] = uiSlots[i].ItemModel;
        }

        public List<InventoryBase> GetInventoryList()
        {
            return new List<InventoryBase> { _inventory };
        }

        public void SetInventoryList(List<InventoryBase> inventoryList)
        {
            if (inventoryList != null && inventoryList.Count > 0)
            {
                if (inventoryList.Count > 0)
                    _inventory = inventoryList[0];

                _slotInited = true;
            }
        }
    }
}
