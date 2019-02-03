using Assets.Scripts.Models.Events;
using System;
using System.Collections.Generic;
namespace Assets.Scripts.Models
{
    public class InventoryBase
    {
        public const string INVENTORY_ADD_ITEM = "INVENTORY_ADD_ITEM";

        public int MaxSlots { get; set; }
        public List<HolderObject> Slots = new List<HolderObject>();

        protected SimpleEvents _simpleEvents = new SimpleEvents();

        public virtual void Init(int maxSlots = 0)
        {
            MaxSlots = maxSlots;
            PrepareSlots(MaxSlots);
        }

        public void PrepareSlots(int maxSlots)
        {
            MaxSlots = maxSlots;

            for (int i = 0; i < MaxSlots; i++)
                Slots.Add(null);
        }

        public void AddNewSlots(int amount)
        {
            MaxSlots += amount;

            for (int i = 0; i < amount; i++)
                Slots.Add(null);
        }

        public virtual bool AddItem(HolderObject item)
        {
            if (AddItem(item, Slots, MaxSlots))
            {
                _simpleEvents.Call(INVENTORY_ADD_ITEM, item);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool AddItem(HolderObject item, List<HolderObject> slots, int maxSlots)
        {
            if (item.Item.IsStackable)
            {
                for (int i = 0; i < maxSlots; i++)
                {
                    if (slots[i] != null && slots[i].Item != null && slots[i].Item.GetType() == item.Item.GetType())
                    {
                        slots[i].Add(item.Amount);
                        return true;
                    }
                }
            }
            
            for (int i = 0; i < maxSlots; i++)
            {
                if (slots[i] == null || slots[i].Item == null)
                {
                    slots[i] = item;
                    return true;
                }
            }

            return false;
        }

        public bool CheckItems(List<HolderObject> items)
        {
            foreach (var holderObject in items)
                if (!CheckItem(holderObject))
                    return false;

            return true;
        }

        public virtual bool CheckItem(HolderObject item)
        {
            int amount = 0;

            for (int i = 0; i < MaxSlots; i++)
            {
                if (Slots[i] != null && Slots[i].Item.GetType() == item.Item.GetType())
                {
                    amount += Slots[i].Amount;
                }
            }

            return amount >= item.Amount;
        }

        public void RemoveItems(List<HolderObject> items)
        {
            foreach (var holderObject in items)
                RemoveItem(holderObject);
        }

        public virtual void RemoveItem(HolderObject item, int? changeAmount = null)
        {
            int amount = changeAmount ?? item.Amount;

            for (int i = 0; i < MaxSlots; i++)
            {
                if (Slots[i] != null && Slots[i].Item != null && Slots[i].Item.GetType() == item.Item.GetType() && amount > 0)
                {
                    amount = Slots[i].ChangeAmount(amount);
                    _simpleEvents.Call(INVENTORY_ADD_ITEM, item);
                }
            }
        }

        public virtual void UseItem(GameManager gameManager, Type itemType)
        {
            for (int i = 0; i < MaxSlots; i++)
            {
                if (Slots[i] != null && Slots[i].Item.GetType() == itemType)
                {
                    Slots[i].Use(gameManager);
                    _simpleEvents.Call(INVENTORY_ADD_ITEM, Slots[i]);
                    return;
                }
            }
        }

        public virtual int GetAmount(Type itemType)
        {
            int amount = 0;

            for (int i = 0; i < MaxSlots; i++)
                if (Slots[i] != null && Slots[i].Item.GetType() == itemType && amount > 0)
                    amount += Slots[i].Amount;

            return amount;
        }
    }
}
