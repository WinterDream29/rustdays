using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class UFOManager : MonoBehaviour
    {
        public GameManager GameManager;
        public List<UFOController> UFOS;
        public TOD_Time Time;
        public List<Transform> IslandPoints;

        private Dictionary<Transform, bool> _busyPoints;

        void Start ()
        {
            _busyPoints = new Dictionary<Transform, bool>();
            foreach (var islandPoint in IslandPoints)
                _busyPoints[islandPoint] = false;

            foreach (var ufoController in UFOS)
                ufoController.Init(GameManager);

            StartCoroutine(DelayInit());
        }

        private IEnumerator DelayInit()
        {
            yield return new WaitForSeconds(2.0f);

            Time.OnSunset += SunSet;
            Time.OnSunrise += SunRise;
        }

        void Update()
        {

        }

        private void SunSet()
        {
            var isStart = Random.Range(0, 100);
            if (isStart > 70)
                return;
            foreach (var ufo in UFOS)
            {
                if(ufo.CurrentState != UFOState.InSpace)
                    continue;

                var rand = Random.Range(0, IslandPoints.Count);
                if (!_busyPoints[IslandPoints[rand]])
                {
                    StartCoroutine(ufo.MoveToPoint(IslandPoints[rand].position));
                    _busyPoints[IslandPoints[rand]] = true;
                }
                else
                {
                    foreach (var islandPoint in IslandPoints)
                    {
                        if (!_busyPoints[islandPoint])
                        {
                            StartCoroutine(ufo.MoveToPoint(islandPoint.position));
                            _busyPoints[islandPoint] = true;
                            break;
                        }
                    }
                }
            }
        }

        private void SunRise()
        {
            foreach (var ufo in UFOS)
            {
                if (ufo.CurrentState == UFOState.OnIsland)
                    ufo.StartMoveAliensToUfo();
                foreach (var islandPoint in IslandPoints)
                    _busyPoints[islandPoint] = false;
            }
        }

        void OnDestroy()
        {
            Time.OnSunset -= SunSet;
            Time.OnSunrise -= SunRise;
        }
    }
}