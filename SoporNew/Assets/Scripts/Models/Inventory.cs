using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models
{
    public class Inventory : InventoryBase
    {
        public const string INVENTORY_ADD_QUICK_SLOT_ITEM = "INVENTORY_ADD_QUICK_SLOT_ITEM";

        public List<HolderObject> QuickSlots = new List<HolderObject>();
        public List<HolderObject> EquipSlots = new List<HolderObject>();

        public override void Init(int maxSlots = 0)
        {
            PrepareQuickSlots();
            PrepareEquipSlots();
        }

        private void PrepareQuickSlots()
        {
            for (int i = 0; i < WorldConsts.QuickSlotsAmount; i++)
                QuickSlots.Add(null);
        }

        private void PrepareEquipSlots()
        {
            for(int i = 0; i < WorldConsts.EquipSlotsAmount; i++)
                EquipSlots.Add(null);
        }

        //public void AddToQuick(HolderObject item)
        //{
        //    AddItem(item, QuickSlots, WorldConsts.QuickSlotsAmount);
        //    _simpleEvents.Call(INVENTORY_ADD_QUICK_SLOT_ITEM, item);
        //}

        public override bool AddItem(HolderObject item)
        {
            if (AddItem(item, Slots, MaxSlots))
            {
                _simpleEvents.Call(INVENTORY_ADD_ITEM, item);
                return true;
            }

            if(AddItem(item, QuickSlots, WorldConsts.QuickSlotsAmount))
            {
                _simpleEvents.Call(INVENTORY_ADD_QUICK_SLOT_ITEM, item);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void UseItem(GameManager gameManager, Type itemType)
        {
            for (int i = 0; i < MaxSlots; i++)
            {
                if (Slots[i] != null && Slots[i].Item != null && Slots[i].Item.GetType() == itemType)
                {
                    Slots[i].Use(gameManager);
                    _simpleEvents.Call(INVENTORY_ADD_ITEM, Slots[i]);
                    return;
                }
            }

            for (int i = 0; i < WorldConsts.QuickSlotsAmount; i++)
            {
                if (QuickSlots[i] != null && QuickSlots[i].Item != null && QuickSlots[i].Item.GetType() == itemType)
                {
                    QuickSlots[i].Use(gameManager);
                    _simpleEvents.Call(INVENTORY_ADD_QUICK_SLOT_ITEM, QuickSlots[i]);
                    return;
                }
            }
        }

        public override bool CheckItem(HolderObject item)
        {
            int amount = 0;

            for (int i = 0; i < MaxSlots; i++)
            {
                if (Slots[i] != null && Slots[i].Item.GetType() == item.Item.GetType())
                {
                    amount += Slots[i].Amount;
                }
            }
            for (int i = 0; i < WorldConsts.QuickSlotsAmount; i++)
            {
                if (QuickSlots[i] != null && QuickSlots[i].Item.GetType() == item.Item.GetType())
                {
                    amount += QuickSlots[i].Amount;
                }
            }

            return amount >= item.Amount;
        }

        public override void RemoveItem(HolderObject item, int? changeAmount = null)
        {
            int amount = changeAmount ?? item.Amount;

            for (int i = 0; i < MaxSlots; i++)
            {
                if (Slots[i] != null && item != null && item.Item != null && Slots[i].Item.GetType() == item.Item.GetType() && amount > 0)
                {
                    amount = Slots[i].ChangeAmount(amount);
                    _simpleEvents.Call(INVENTORY_ADD_ITEM, item);
                }
            }
            for (int i = 0; i < WorldConsts.QuickSlotsAmount; i++)
            {
                if (QuickSlots[i] != null && item != null && item.Item != null && QuickSlots[i].Item.GetType() == item.Item.GetType() && amount > 0)
                {
                    amount = QuickSlots[i].ChangeAmount(amount);
                    _simpleEvents.Call(INVENTORY_ADD_QUICK_SLOT_ITEM, item);
                }
            }
        }

        public override int GetAmount(Type itemType)
        {
            int amount = 0;

            for (int i = 0; i < MaxSlots; i++)
                if (Slots[i] != null && Slots[i].Item.GetType() == itemType)
                    amount += Slots[i].Amount;

            for (int i = 0; i < WorldConsts.QuickSlotsAmount; i++)
                if (QuickSlots[i] != null && QuickSlots[i].Item.GetType() == itemType)
                    amount += QuickSlots[i].Amount;

            return amount;
        }
    }
}
