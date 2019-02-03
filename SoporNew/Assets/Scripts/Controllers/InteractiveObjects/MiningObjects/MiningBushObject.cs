using Assets.Scripts.Models;
using Assets.Scripts.Models.ResourceObjects;

namespace Assets.Scripts.Controllers.InteractiveObjects.MiningObjects
{
    public class MiningBushObject : MiningBaseObject
    {
        public override HolderObject GetResource()
        {
            var amount = GetMiningAmount();
            InitialAmount -= amount;
            return HolderObjectFactory.GetItem(typeof(FiberResource), amount);
        }
    }
}
