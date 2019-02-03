using UnityEngine;

namespace Assets.Scripts.Controllers.Constructions
{
    public class DoorController : MonoBehaviour
    {
        public Animation DoorAnimation;

        private bool _isOpen;
        private GameManager _gameManager;

        public void OpenDoor()
        {
            DoorAnimation.Play(_isOpen ? "CloseDoor" : "OpenDoor");
            _isOpen = !_isOpen;
        }

        void OnMouseDown()
        {
            if (_gameManager == null)
                _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

            if (_gameManager != null)
            {
                var playerDistance = (transform.position - _gameManager.Player.transform.position).sqrMagnitude;
                if(playerDistance < 20.0f)
                    OpenDoor();
            }
            else
            {
                OpenDoor();
            }
        }
    }
}
