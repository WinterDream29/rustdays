using System;

namespace Assets.Scripts.Models
{
    public static class HolderObjectFactory
    {
        public static HolderObject GetItem(Type itemType, int amount)
        {
            return new HolderObject(itemType, amount);
        }

        public static HolderObject GetItem(Type itemType, int amount, int? durability)
        {
            return new HolderObject(itemType, amount, durability);
        }

        public static HolderObject GetItem(string itemTypeName, int amount, int? durability = null)
        {
            return new HolderObject(itemTypeName, amount, durability);
        }
    }
}
