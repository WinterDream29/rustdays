namespace Assets.Scripts.SaveModels
{
    public class GroundItemSaveModel
    {
        public string ItemName;
        public int CurrentHp;
        public bool IsActive;

        public float PosX;
        public float PosY;
        public float PosZ;

        public float Pitch;
        public float Roll;
        public float Yaw;

        public int Amount;
        public int? Durability;
        public bool Dropped;

        public int AmountFilled;

        public InventoryBaseSaveModelList InventoryList;
    }
}
