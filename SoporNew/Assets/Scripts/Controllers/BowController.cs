using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public enum BowState
    {
        Idle,
        Pull,
        Ready
    }

    public class BowController : MonoBehaviour 
    {
        public Animation AttackAnimation;
        public GameObject ArrowPrefab;
        public Transform ArrowPosition;
        public float PullTime = 4f;


        public BowState CurrentState = BowState.Idle;

        private ArrowController _arrowController;
        private float _currentPullTime;
        private GameManager _gameManager;

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void TetevenPull()
        {
            _currentPullTime = 0;

            var arrowGo = Instantiate(ArrowPrefab);
            arrowGo.transform.parent = ArrowPosition;
            arrowGo.transform.localPosition = Vector3.zero;
            arrowGo.transform.localRotation = Quaternion.identity;
            _arrowController = arrowGo.GetComponent<ArrowController>();
            _arrowController.Init(_gameManager);

            AttackAnimation.Play("Pull");

            CurrentState = BowState.Pull;
            SoundManager.PlaySFX(WorldConsts.AudioConsts.BowString, false, 0.3f);
        }

        public void BowReadyToShoot()
        {
            CurrentState = BowState.Ready;
        }

        public void TetevenWeaken()
        {
            AttackAnimation.Play("Weaken");
            CurrentState = BowState.Idle;
            Destroy(_arrowController.gameObject);
            _arrowController = null;
        }

        public void Shoot(Camera playerCamera)
        {
            if (CurrentState == BowState.Ready)
            {
                _arrowController.Shoot();
                _arrowController = null;
                CurrentState = BowState.Idle;
                StartCoroutine(DelayShoot());
            }
        }

        private IEnumerator DelayShoot()
        {
            yield return new WaitForSeconds(0.1f);
            AttackAnimation.Play("Shoot");
            SoundManager.PlaySFX(WorldConsts.AudioConsts.BowRelease);
        }

        void Update()
        {
            if (CurrentState == BowState.Pull)
            {
                _currentPullTime += Time.deltaTime;
                if (_currentPullTime >= PullTime)
                {
                    _currentPullTime = 0f;
                    TetevenWeaken();
                }
            }
        }
    }
}
