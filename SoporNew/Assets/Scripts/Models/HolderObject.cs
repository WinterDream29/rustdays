using System;

namespace Assets.Scripts.Models
{
    public class HolderObject
    {
        public BaseObject Item { get; private set; }
        public int Amount { get; private set; }
        public int? CurrentDurability { get; private set; }

        public Action<HolderObject> OnRemoved { get; set; }
        public Action OnDurabilityChanged { get; set; }
        public Action OnAmountChanged { get; set; }

        public HolderObject(Type itemType, int amount, int? durability = null)
        {
            Item = BaseObjectFactory.GetItem(itemType);
            Amount = amount;
            if (durability == null || durability == 0)
                CurrentDurability = Item.Durability;
            else
                CurrentDurability = durability;
        }

        public HolderObject(string itemTypeName, int amount, int? durability = null)
        {
            Item = BaseObjectFactory.GetItem(itemTypeName);
            Amount = amount;
            if (durability == null || durability == 0)
                CurrentDurability = Item.Durability;
            else
                CurrentDurability = durability;
        }

        public void Add(int amount)
        {
            Amount += amount;
        }

        public void SetAmount(int amount)
        {
            Amount = amount;
        }

        public int ChangeAmount(int amount, bool sendEvent = true)
        {
            if (Amount > amount)
            {
                Amount -= amount;

                if (sendEvent)
                {
                    if (OnAmountChanged != null)
                        OnAmountChanged();
                }

                return 0;
            }
            else
            {
                var result = amount - Amount;

                Remove(sendEvent);

                return result;
            }
        }

        public void Remove(bool sendEvent = true)
        {
            if (sendEvent)
            {
                if (OnRemoved != null)
                    OnRemoved(this);
            }

            Item = null;
            Amount = 0;
        }

        public void ChangeDurability(int amount)
        {
            CurrentDurability -= amount;

            if (OnDurabilityChanged != null)
                OnDurabilityChanged();

            if (CurrentDurability <= 0)
                Remove();
        }

        public void Use(GameManager gameManager)
        {
            Item.Use(gameManager, amount => ChangeAmount(amount));
        }
    }
}
