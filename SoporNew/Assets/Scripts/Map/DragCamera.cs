using UnityEngine;

namespace Assets.Scripts.Map
{
    public class DragCamera : MonoBehaviour
    {
        public float DragSpeed = 2;
        public float OuterLeft = -10f;
        public float OuterRight = 10f;
        public float OuterTop = 10f;
        public float OuterDown = 10f;

        private Vector3 _dragOrigin;
        public bool CameraDragging = true;

        void Update ()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _dragOrigin = Input.mousePosition;
                return;
            }

            if (!Input.GetMouseButton(0)) return;

            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _dragOrigin);
            Vector3 move = new Vector3(pos.x * DragSpeed, 0, pos.y * DragSpeed);

            transform.Translate(move, Space.World);
        }
    }
}