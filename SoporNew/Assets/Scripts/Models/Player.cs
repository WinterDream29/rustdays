using System;
using Assets.Scripts.Models.Events;
using Assets.Scripts.UI;

namespace Assets.Scripts.Models
{
    public enum BackpackType
    {
        None = 0,
        Small = 1,
        Big = 2,
    }

    public class Player
    {
        public const string PLAYER_UPDATE_STATS = "PLAYER_UPDATE_STATS";
        public const string PLAYER_GET_DAMAGE= "PLAYER_GET_DAMAGE";
        public const string PLAYER_UNDER_WATER = "PLAYER_UNDER_WATER";

        public float Health { get; set; }
        public float Hunger { get; set; }
        public float Thirst { get; set; }
        public float Energy { get; set; }
        public float Breath { get; set; }
        public BackpackType CurrentBackpack { get; set; }

        public Inventory Inventory { get; private set; }

        public float HungerLifeDecSpeed { get; protected set; }
        public float ThirstLifeDecSpeed { get; protected set; }
        public float O2LifeDecSpeed { get; protected set; }

        public bool Dead { get; set; }
        public bool Sleep { get; protected set; }
        public bool UnderWater { get; private set; }

        public Action OnDeathAction { get; set; }
        public Action OnSleepAction { get; set; }

        private SimpleEvents _simpleEvents = new SimpleEvents();

        private GameManager _gameManager;

        private bool _isSleepped;

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;

            Inventory = new Inventory();
            Inventory.Init();

            HungerLifeDecSpeed = 0.4f;
            ThirstLifeDecSpeed = 0.4f;
            O2LifeDecSpeed = 3.0f;
        }

        public void AddSleepEnergy(TimeSpan timeLogout)
        {
            const float recoveryStep = 100.0f / 3600.0f;
            Energy += ((float)timeLogout.TotalSeconds) * recoveryStep;
            if (Energy > 100.0f)
                Energy = 100.0f;
        }

        public void ChangeItemAmount(UiSlot slot, int amount)
        {
            switch (slot.SlotType)
            {
                case SlotType.Inventory:
                    Inventory.Slots[slot.SlotId].ChangeAmount(amount);
                    break;
                case SlotType.Quick:
                    Inventory.QuickSlots[slot.SlotId].ChangeAmount(amount);
                    break;
                case SlotType.EquipBoots:
                case SlotType.EquipCap:
                case SlotType.EquipPants:
                case SlotType.EquipShirt:
                    Inventory.EquipSlots[slot.SlotId].ChangeAmount(amount);
                    break;
                case SlotType.Interactive:
                    if (_gameManager.DisplayManager.CurrentInteractPanel != null && _gameManager.DisplayManager.CurrentInteractPanel is InteractView)
                    {
                        var panel = _gameManager.DisplayManager.CurrentInteractPanel as InteractView;
                        panel.ChageAmount(slot, amount);
                    }
                    break;
            }
        }

        public void SetItem(UiSlot slot, HolderObject itemModel)
        {
            if (slot.SlotType == SlotType.Inventory)
            {
                Inventory.Slots[slot.SlotId] = itemModel;
            }
            else if (slot.SlotType == SlotType.Quick)
            {
                Inventory.QuickSlots[slot.SlotId] = itemModel;
            }
            else if (slot.SlotType == SlotType.EquipBoots ||
                     slot.SlotType == SlotType.EquipCap ||
                     slot.SlotType == SlotType.EquipPants ||
                     slot.SlotType == SlotType.EquipShirt)
            {
                Inventory.EquipSlots[slot.SlotId] = itemModel;
            }
            else if (slot.SlotType == SlotType.Interactive)
            {
                if (_gameManager.DisplayManager.CurrentInteractPanel != null && _gameManager.DisplayManager.CurrentInteractPanel is InteractView)
                {
                    var panel = _gameManager.DisplayManager.CurrentInteractPanel as InteractView;
                    panel.SetItem(slot, itemModel);
                }
                if (slot.OnValueChanged != null)
                    slot.OnValueChanged(slot);
            }
            else
            {
                if (slot.OnValueChanged != null)
                    slot.OnValueChanged(slot);
            }
        }

        public void UpdateStates()
        {
            if (Dead)
                return;

            if (Energy > 0)
                Energy -= 0.06f;
            else
                Energy = 0.0f;

            if (Thirst > 0)
                Thirst -= 0.09f;
            else
                Thirst = 0.0f;

            if (Hunger > 0)
                Hunger -= 0.07f;
            else
                Hunger = 0.0f;

            if (UnderWater)
            {
                if (Breath > 0)
                    Breath -= 5f;
                else
                    Breath = 0;
            }
            else
            {
                Breath = 100f;
            }

            if (Thirst <= 0.0f)
                ChangeHealth(-ThirstLifeDecSpeed);
            if (Hunger <= 0.0f)
                ChangeHealth(-HungerLifeDecSpeed);
            if (Breath <= 0.0f)
                ChangeHealth(-O2LifeDecSpeed);



            _simpleEvents.Call(PLAYER_UPDATE_STATS, null);

            CheckIfSleep();
        }

        public void ChangeHealth(float amount)
        {
            Health += amount;
            if (Health > 100f)
                Health = 100f;
            if (Health <= 0f)
            {
                Health = 0f;
                Dead = true;
                if (OnDeathAction != null)
                    OnDeathAction();

                SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerDead);
            }

            _simpleEvents.Call(PLAYER_UPDATE_STATS, null);
        }

        public void Damage(float damage)
        {
            var defence = 0.0f;
            foreach(var slot in Inventory.EquipSlots)
            {
                if (slot != null && slot.Item != null && slot.Item.Effect == ItemEffectType.Damage)
                    defence += slot.Item.EffectAmount;
            }

            defence = defence / 100.0f;
            if (defence >= 100.0f)
                defence = 95.0f;

            var percentDamage = damage * defence;
            damage -= percentDamage;
            if (damage <= 0)
                damage = 5f;

            ChangeHealth(-damage);
            _simpleEvents.Call(PLAYER_GET_DAMAGE, null);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerHurt);
        }

        public void ChangeEnergy(float amount)
        {
            Energy += amount;
            if (Energy > 100f)
                Energy = 100f;
            if (Energy < 0f)
                Energy = 0f;

            _simpleEvents.Call(PLAYER_UPDATE_STATS, null);
        }

        public void ChangeThirst(float amount)
        {
            Thirst += amount;
            if (Thirst > 100f)
                Thirst = 100f;
            if (Thirst < 0f)
                Thirst = 0f;

            _simpleEvents.Call(PLAYER_UPDATE_STATS, null);
        }

        public void ChangeHunger(float amount)
        {
            Hunger += amount;
            if (Hunger > 100f)
                Hunger = 100f;
            if (Hunger < 0f)
                Hunger = 0f;

            _simpleEvents.Call(PLAYER_UPDATE_STATS, null);
        }

        private void CheckIfSleep()
        {
            Sleep = Energy <= 0;

            if (!Sleep)
                _isSleepped = false;

            if (Sleep && !_isSleepped)
            {
                _isSleepped = true;

                if (OnSleepAction != null)
                    OnSleepAction();
            }
        }

        public void SetUnderWater(bool under)
        {
            UnderWater = under;
            _simpleEvents.Call(PLAYER_UNDER_WATER, under);
        }

        public void PrepareInventorySlots()
        {
            var amount = GetAmountSlotsForBackpack();
            Inventory.PrepareSlots(amount);
        }

        public void BiggestBackpack()
        {
            CurrentBackpack = (BackpackType)((int)CurrentBackpack + 1);
            var additional = GetAdditionalAmountSlotsForBackpack();
            Inventory.AddNewSlots(additional);
            _gameManager.Player.MainHud.InventoryPanel.AddNewSlots(additional);
        }

        private int GetAmountSlotsForBackpack()
        {
            switch (CurrentBackpack)
            {
                case BackpackType.None:
                    return WorldConsts.BackpackAmountSlost;
                    break;
                case BackpackType.Small:
                    return WorldConsts.BackpackAmountSlost + WorldConsts.BackpackSmallAdditionalSlost;
                    break;
                case BackpackType.Big:
                    return WorldConsts.BackpackAmountSlost + WorldConsts.BackpackSmallAdditionalSlost + WorldConsts.BackpackBigAdditionalSlots;
                    break;
            }
            return 0;
        }

        private int GetAdditionalAmountSlotsForBackpack()
        {
            switch (CurrentBackpack)
            {
                case BackpackType.None:
                    return WorldConsts.BackpackAmountSlost;
                    break;
                case BackpackType.Small:
                    return WorldConsts.BackpackSmallAdditionalSlost;
                    break;
                case BackpackType.Big:
                    return WorldConsts.BackpackBigAdditionalSlots;
                    break;
            }
            return 0;
        }
    }
}
