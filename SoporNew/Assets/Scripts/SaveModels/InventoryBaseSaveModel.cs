using System.Collections.Generic;
namespace Assets.Scripts.SaveModels
{
    public class InventoryBaseSaveModel
    {
        public int SlotAmount;
        public List<ItemHolderSaveModel> Items = new List<ItemHolderSaveModel>();
    }
}
