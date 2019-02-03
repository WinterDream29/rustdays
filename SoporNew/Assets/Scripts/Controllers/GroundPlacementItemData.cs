using Assets.Scripts.Models;

namespace Assets.Scripts.Controllers
{
    public class GroundPlacementItemData
    {
        public BaseObject ItemModel;
        public bool Dropped;
        public int Amount;
        public int? Durability;

        public GroundPlacementItemData(BaseObject itemModel, bool dropped = false, int amount = 1, int? durability = null)
        {
            ItemModel = itemModel;
            Dropped = dropped;
            Amount = amount;
            Durability = durability;
        }
    }
}
