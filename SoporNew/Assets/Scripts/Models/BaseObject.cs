using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models
{
    public enum ItemConverterType
    {
        Campfire,
        Furnace,
        None
    }

    public enum ItemEffectType
    {
        None,
        Damage,
        Cold,
        Radiation
    }

    public struct IntVector2
    {
        public int X;
        public int Y;

        public IntVector2(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public abstract class BaseObject
    {
        public string LocalizationName { get; protected set; }
        public string Description { get; protected set; }
        public string IconName { get; protected set; }
        public int CraftAmount { get; protected set; }
        public int? Durability { get; protected set; }
        public bool ShowDurability { get; protected set; }
        public int NeedWoodToCook { get; protected set; }
        public float AttackFirstHalfTime { get; protected set; }
        public float AttackSeconfHalfTime { get; protected set; }
        public float ChangeWeaponTime { get; protected set; }
        public bool IsStackable { get; protected set; }
        public int Damage { get; protected set; }
        public string OnGroundPrefabPath { get; protected set; }
        public int MiningAdditionalAmount { get; protected set; }
        public int ShopPrice { get; protected set; }
        public bool CanBuy { get; protected set; }

        public List<HolderObject> CraftRecipe { get; protected set; }
        public List<ItemConverterType> Converters { get; protected set; }
        public KeyValuePair<BaseObject, int> CookingResult { get; protected set; }
        public KeyValuePair<BaseObject, int> GardenResult { get; protected set; }
        public KeyValuePair<BaseObject, int> GardenWitheredResult { get; protected set; }
        public string GardenPlacedPrefabStage1 { get; protected set; }
        public string GardenPlacedPrefabStage2 { get; protected set; }
        public string GardenPlacedPrefabStage3 { get; protected set; }
        public string GardenPlacedPrefabWithered { get; protected set; }
        public int GardenTimeStage1 { get; protected set; }
        public int GardenTimeStage2 { get; protected set; }
        public int GardenTimeStage3 { get; protected set; }
        public ItemEffectType Effect { get; protected set; }
        public float EffectAmount { get; protected set; }
        public bool AddDestroyReward { get; protected set; }

        protected BaseObject()
        {
            Durability = null;
            ShowDurability = true;
            CraftAmount = 1;
            IsStackable = true;
            OnGroundPrefabPath = "Prefabs/Items/Bag/Bag";
            MiningAdditionalAmount = 0;
            NeedWoodToCook = 1;
            Effect = ItemEffectType.None;
            EffectAmount = 0.0f;
            AddDestroyReward = true;
        }

        public virtual void Use(GameManager gameManager, Action<int> changeAmount = null)
        {
            
        }
    }
}
