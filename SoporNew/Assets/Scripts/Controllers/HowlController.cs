using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class HowlController : MonoBehaviour 
    {
        public List<AudioClip> Howls;
        public List<AudioSource> HowlAudios;

        public float WaitTimeNextHowlMin = 10f;
        public float WaitTimeNextHowlMax = 20f;

        private bool _howlTime = false;
        private bool _howling = false;
        private Coroutine _howlCor;

        void Start()
        {
            StartCoroutine(CheckHowlTime());
        }

        public IEnumerator StartHowl()
        {
            _howling = true;
            while(true)
            {
                foreach (var source in HowlAudios)
                {
                    var waitTime = Random.Range(1, 10);
                    var clipId = Random.Range(0, Howls.Count);

                    source.PlayOneShot(Howls[clipId]);
                    yield return new WaitForSeconds(waitTime);
                }

                var waitTime2 = Random.Range(WaitTimeNextHowlMin, WaitTimeNextHowlMax);
                yield return new WaitForSeconds(waitTime2);
            }
        }

        public void StopHowl()
        {
            StopCoroutine(_howlCor);
            _howling = false;
        }

        private IEnumerator CheckHowlTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(5.0f);
                _howlTime = TOD_Sky.Instance.Cycle.Hour > 0 && TOD_Sky.Instance.Cycle.Hour < 2;

                if (_howlTime && !_howling)
                    _howlCor = StartCoroutine(StartHowl());
                else if (!_howlTime && _howling)
                    StopHowl();
            }
        }

        void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}
