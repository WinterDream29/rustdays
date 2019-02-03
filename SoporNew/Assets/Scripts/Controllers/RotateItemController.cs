using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class RotateItemController : MonoBehaviour
    {
        public float RotationSpeed = 10.0f;
        public float LerpSpeed = 100.0f;

        private Vector3 _speed;
        private Vector3 _avgSpeed;
        private bool _dragging;
        private Vector3 _targetSpeedX;

        private GameManager _gameManager;

        void Start()
        {
            _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }

        void OnMouseDown()
        {
            _dragging = true;
        }

        void Update()
        {
            if ((Input.GetMouseButton(0) /*|| Input.GetTouch(0).phase == TouchPhase.Began*/) && _dragging)
            {
                _speed = new Vector3(-Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
                _avgSpeed = Vector3.Lerp(_avgSpeed, _speed, Time.deltaTime * 5);
                //if (_gameManager != null && _gameManager.Player.MainHud.LookPadCollider.enabled)
                //    _gameManager.Player.MainHud.LookPadCollider.gameObject.SetActive(false);
            }
            else
            {
                if (_dragging)
                {
                    _speed = _avgSpeed;
                    _dragging = false;
                }
                var i = Time.deltaTime * LerpSpeed;
                _speed = Vector3.Lerp(_speed, Vector3.zero, i);

                //if (_gameManager != null && !_gameManager.Player.MainHud.LookPadCollider.gameObject.activeSelf)
                //    _gameManager.Player.MainHud.LookPadCollider.gameObject.SetActive(true);
            }

            transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y * _speed.x * RotationSpeed, transform.rotation.z));
            //transform.Rotate(Camera.main.transform.up * _speed.x * RotationSpeed, Space.World);
            //transform.Rotate(Camera.main.transform.right * speed.y * rotationSpeed, Space.World);
        }
    }
}