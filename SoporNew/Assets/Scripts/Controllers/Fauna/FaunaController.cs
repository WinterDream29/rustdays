using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers.Fauna
{
    public class FaunaController : MonoBehaviour
    {
        public List<DistanceByQuality> DisableAnimalDistance;

        private List<FaunaZone> _zones = new List<FaunaZone> ();
        private GameManager _gameManager;

        public float HideAnimalDistance { get; private set; }

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;

            StartCoroutine(InitZones());

            SetDistance(QualityManager.CurrentQuality);
            QualityManager.OnChangeQuality += SetDistance;
        }

        public void SetDistance(QualityType type)
        {
            foreach (var distanceByQuality in DisableAnimalDistance)
            {
                if (distanceByQuality.Type == type)
                {
                    HideAnimalDistance = distanceByQuality.Distance;
                    break;
                }
            }
        }

        private IEnumerator InitZones()
        {
			_zones.AddRange(gameObject.GetComponentsInChildren<FaunaZone>());
			
            foreach (var resSpawnZone in _zones)
            {
                resSpawnZone.Init(_gameManager, this);
                yield return new WaitForEndOfFrame();
            }

            yield break;
        }

        void OnDestroy()
        {
            if(QualityManager.OnChangeQuality != null)
                QualityManager.OnChangeQuality -= SetDistance;
        }
    }

    [Serializable]
    public class DistanceByQuality
    {
        public QualityType Type;
        [Range(100, 50000)]
        public float Distance;
    }
}
