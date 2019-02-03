using System;
using CodeStage.AntiCheat.ObscuredTypes;

namespace Assets.Scripts
{
    public class CurrencyManager
    {
        public static Action OnChangeCurrency;
        public static ObscuredInt CurrentCurrency { get; private set; }

        public static void AddCurrency(int amount)
        {
            CurrentCurrency += amount;
            if (OnChangeCurrency != null)
                OnChangeCurrency();
        }

        public static void SetCurrency(int amount)
        {
            CurrentCurrency = amount;
            if (OnChangeCurrency != null)
                OnChangeCurrency();
        }
    }
}
