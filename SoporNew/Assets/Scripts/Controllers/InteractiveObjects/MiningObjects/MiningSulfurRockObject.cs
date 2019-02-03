using Assets.Scripts.Models;
using Assets.Scripts.Models.ResourceObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Controllers.InteractiveObjects.MiningObjects
{
    public class MiningSulfurRockObject : MiningRockObject
    {
        public override HolderObject GetResource()
        {
            var amount = GetMiningAmount();

            InitialAmount -= GetMiningAmount();

            var precent = Random.Range(0, 100);
            if (precent < 40)
                return HolderObjectFactory.GetItem(typeof(SulfurOre), amount);

            return HolderObjectFactory.GetItem(typeof(StoneResource), amount);
        }
    }
}