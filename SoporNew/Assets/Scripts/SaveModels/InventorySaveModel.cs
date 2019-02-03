using System.Collections.Generic;

namespace Assets.Scripts.SaveModels
{
    public class InventorySaveModel
    {
        public Dictionary<string, string> ItemInSlots = new Dictionary<string, string>();
        public Dictionary<string, int> ItemAmountInSlots = new Dictionary<string, int>();
        public Dictionary<string, int?> ItemDurabilityInSlots = new Dictionary<string, int?>();

        public Dictionary<string, string> ItemInQuickSlots = new Dictionary<string, string>();
        public Dictionary<string, int> ItemAmountInQuickSlots = new Dictionary<string, int>();
        public Dictionary<string, int?> ItemDurabilityInQuickSlots = new Dictionary<string, int?>();

        public Dictionary<string, string> ItemInEquipSlots = new Dictionary<string, string>();
        public Dictionary<string, int> ItemAmountInEquipSlots = new Dictionary<string, int>();
        public Dictionary<string, int?> ItemDurabilityInEquipSlots = new Dictionary<string, int?>();
    }
}
