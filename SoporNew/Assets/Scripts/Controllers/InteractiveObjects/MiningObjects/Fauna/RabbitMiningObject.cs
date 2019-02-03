using Assets.Scripts.Models;
using Assets.Scripts.Models.Food;
using Assets.Scripts.Models.ResourceObjects;
using UnityEngine;

namespace Assets.Scripts.Controllers.InteractiveObjects.MiningObjects.Fauna
{
    public class RabbitMiningObject : MiningAnimalObject
    {
        public override HolderObject GetResource()
        {
            var amount = Random.Range(MinMiningAmount, MaxMiningAmount);
            InitialAmount -= amount;

            var value = Random.Range(0, 100);
            if(value < 60)
                return HolderObjectFactory.GetItem(typeof(Meat), amount);

            return HolderObjectFactory.GetItem(typeof(Fur), amount);
        }
    }
}
