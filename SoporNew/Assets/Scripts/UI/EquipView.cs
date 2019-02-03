using Assets.Scripts.Ui;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class EquipView : View
    {
        public List<UiSlot> Slots;
        public UISprite ShieldProgress;
        public Action<UiSlot> OnSlotClickAction { get; set; }

        private float _currentWaitUpdate = 0.0f; 

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);

            for (int i = 0; i < WorldConsts.EquipSlotsAmount; i++)
            {
                Slots[i].Init(gameManager, GameManager.PlayerModel.Inventory.EquipSlots[i], i);
                Slots[i].OnSlotClickAction += OnSlotClick;
            }
        }

        void Update()
        {
            if (IsShowing)
            {
                _currentWaitUpdate += Time.deltaTime;
                if (_currentWaitUpdate > 1.0f)
                {
                    _currentWaitUpdate = 0.0f;
                    UpdateDefence();
                }
            }
        }

        private void OnSlotClick(UiSlot uiSlot)
        {
            if (OnSlotClickAction != null)
                OnSlotClickAction(uiSlot);
        }

        public override void UpdateView()
        {
            base.UpdateView();

            for (int i = 0; i < WorldConsts.EquipSlotsAmount; i++)
                Slots[i].UpdateView();

            UpdateDefence();
        }

        private void UpdateDefence()
        {
            float amount = 0;
            foreach(var slot in Slots)
            {
                if(slot.ItemModel != null && slot.ItemModel.Item != null)
                {
                    if (slot.ItemModel.Item.Effect == Models.ItemEffectType.Damage)
                        amount += slot.ItemModel.Item.EffectAmount;
                }
            }

            if (amount > 100)
                amount = 100;

            ShieldProgress.fillAmount = amount / 100.0f;
        }

        public void UpdateItemsDurability()
        {
            for (int i = 0; i < WorldConsts.EquipSlotsAmount; i++)
                Slots[i].UpdateDurability();
        }
    }
}
