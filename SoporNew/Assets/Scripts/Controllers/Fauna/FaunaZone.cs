using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers.Fauna
{
    public class FaunaZone : MonoBehaviour
    {
        public MeshRenderer ZoneRender;
        public TerrainId TerrainId;

        public GameObject Prefab1;
        public int Amount1;
        [Range(0, 100)]
        public int AppearChance1;

        public GameObject Prefab2;
        public int Amount2;
        [Range(0, 100)]
        public int AppearChance2;

        public GameObject Prefab3;
        public int Amount3;
        [Range(0, 100)]
        public int AppearChance3;

        public GameObject Prefab4;
        public int Amount4;
        [Range(0, 100)]
        public int AppearChance4;
        public Terrain CurrentTerrain { get; private set; }

        private GameManager _gameManager;
        private List<Animal> _animals = new List<Animal>();
        private FaunaController _manager;

        public void Init(GameManager gameManager, FaunaController manager)
        {
            _gameManager = gameManager;
            _manager = manager;
            CurrentTerrain = TerrainId == TerrainId.Terrain1 ? _gameManager.Terrain1 : _gameManager.Terrain2;

            ZoneRender.enabled = false;
            SpawnAllObjects();
            StartCoroutine(SetVisibleByPlayerDistance());
        }

        private void SpawnAllObjects()
        {
            StartCoroutine(Spawn(Prefab1, Amount1, AppearChance1));
            StartCoroutine(Spawn(Prefab2, Amount2, AppearChance2));
            StartCoroutine(Spawn(Prefab3, Amount3, AppearChance3));
            StartCoroutine(Spawn(Prefab4, Amount4, AppearChance4));
        }

        public IEnumerator Spawn(GameObject prefab, int amount, int appearChance)
        {
            if (prefab == null)
                yield break;

            for (int i = 0; i < amount; i++)
            {
                if (Random.Range(0, 100) < appearChance)
                {
                    var targetPoint = new GameObject();
                    targetPoint.name = "targetPoint";
                    targetPoint.transform.position = GetPointInside();

                    var animalGo = Instantiate(prefab);
                    animalGo.transform.position = GetPointInside();
                    var animal = animalGo.GetComponent<Animal>();
                    animal.Init(_gameManager, this, targetPoint.transform);
                    _animals.Add(animal);

                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForEndOfFrame();
        }

        public Vector3 GetPointInside()
        {
            var targetPoint = transform.localPosition +
                                new Vector3(
                                    Random.Range(transform.localScale.x / 2, -transform.localScale.x / 2), 0.0f,
                                    Random.Range(transform.localScale.z / 2, -transform.localScale.z / 2));
            //targetPoint.y = Terrain.activeTerrain.SampleHeight(targetPoint);
            targetPoint.y = CurrentTerrain.SampleHeight(targetPoint);
            targetPoint.y -= 0.1f;
            return targetPoint;
        }

        private IEnumerator SetVisibleByPlayerDistance()
        {
            while (true)
            {
                foreach (var animal in _animals)
                {
                    if (!animal.IsDead)
                    {
                        var distance = (animal.transform.localPosition - _gameManager.Player.transform.localPosition).sqrMagnitude;
                        animal.gameObject.SetActive(distance < _manager.HideAnimalDistance);
                    }
                }

                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}