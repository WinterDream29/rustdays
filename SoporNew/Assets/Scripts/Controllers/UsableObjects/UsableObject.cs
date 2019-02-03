using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers.UsableObjects
{
    public class UsableObject : MonoBehaviour
    {
        public Collider InteractCollider;
        public GameObject UiInteractObject;
        public GameObject InteractPanelPrefab;
        [Range(0, 100)]
        public float ShowUiDistance;

        protected GameManager GameManager;

        protected float _playerDistance = 300;

        void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
            StartCoroutine(CheckPlayerDistance());
        }

        private IEnumerator CheckPlayerDistance()
        {
            while (true)
            {
                if (UiInteractObject != null)
                {
                    if (GameManager == null)
                        GameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

                    if (GameManager != null)
                    {
                        _playerDistance = (transform.localPosition - GameManager.Player.transform.localPosition).sqrMagnitude;
                        UiInteractObject.SetActive(_playerDistance < ShowUiDistance);
                        InteractCollider.enabled = _playerDistance < ShowUiDistance;

                        if (_playerDistance < ShowUiDistance)
                        {
                            UiInteractObject.transform.LookAt(GameManager.Player.FpsCamera.transform);
                        }
                    }
                }

                yield return new WaitForSeconds(0.15f);
            }
        }

        public virtual void Use(GameManager gameManager)
        {
            GameManager = gameManager;
        }

        void OnMouseDown()
        {
            GameManager gameManager = null;
            if (GameManager == null)
            {
                gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
                Use(gameManager);
            }
            else
                Use(GameManager);
        }

        protected virtual void Destroyed()
        {
        }

        void OnDestroy()
        {
            StopAllCoroutines();
            Destroyed();
        }
    }
}
