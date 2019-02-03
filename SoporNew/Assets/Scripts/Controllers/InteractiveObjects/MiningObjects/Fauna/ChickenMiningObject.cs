using Assets.Scripts.Models;
using Assets.Scripts.Models.Food;
using Assets.Scripts.Models.ResourceObjects;
using UnityEngine;

namespace Assets.Scripts.Controllers.InteractiveObjects.MiningObjects.Fauna
{
    public class ChickenMiningObject : MiningAnimalObject
    {
        public override HolderObject GetResource()
        {
            var amount = Random.Range(MinMiningAmount, MaxMiningAmount);
            InitialAmount -= amount;

            return HolderObjectFactory.GetItem(typeof(Meat), amount);
        }
    }
}
