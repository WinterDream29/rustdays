using Assets.Scripts.Models;
using Assets.Scripts.Models.Food;
using Assets.Scripts.Models.ResourceObjects;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Controllers.InteractiveObjects.MiningObjects.Fauna
{
    public class SheepMiningObject : MiningAnimalObject
    {
        public override HolderObject GetResource()
        {
            var amount = UnityEngine.Random.Range(MinMiningAmount, MaxMiningAmount);
            InitialAmount -= amount;

            var value = Random.Range(0, 100);
            if (value < 35)
                return HolderObjectFactory.GetItem(typeof(Meat), amount);
            if(value < 70)
                return HolderObjectFactory.GetItem(typeof(Fat), amount);

            return HolderObjectFactory.GetItem(typeof(Fur), amount);
        }
    }
}
