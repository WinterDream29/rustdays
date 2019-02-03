using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Models.MiningObjects
{
    public abstract class MiningObject : BaseObject
    {
        protected List<BaseObject> MiningWeapons = new List<BaseObject>();  
        protected Dictionary<BaseObject, Vector2> MiningResult = new Dictionary<BaseObject, Vector2>();

        public virtual BaseObject GetMiningResult()
        {
            var value = Random.Range(0, 100);
            foreach (var result in MiningResult)
                if (value > result.Value[0] && value < result.Value[1])
                    return result.Key;

            return null;
        }
    }
}
