using UnityEngine;

namespace Assets.Scripts.Map
{
    [RequireComponent(typeof(BoxCollider))]
    public class CameraBounds : MonoBehaviour
    {
        public Camera LinkedCamera;

        private BoxCollider _boxCollider;

        private void Start()
        {
            _boxCollider = this.GetComponent<BoxCollider>();
        }

        private void LateUpdate()
        {
            float vertExtent = LinkedCamera.orthographicSize;
            float horizExtent = vertExtent * Screen.width / Screen.height;

            Vector3 linkedCameraPos = LinkedCamera.transform.position;
            Bounds areaBounds = _boxCollider.bounds;

            //LinkedCamera.transform.position = new Vector3(
            //    Mathf.Clamp(linkedCameraPos.x, areaBounds.min.x + horizExtent, areaBounds.max.x - horizExtent),
            //    Mathf.Clamp(linkedCameraPos.y, areaBounds.min.y + vertExtent, areaBounds.max.y - vertExtent),
            //    linkedCameraPos.z);

            LinkedCamera.transform.position = new Vector3(
                Mathf.Clamp(linkedCameraPos.x, areaBounds.min.x + horizExtent, areaBounds.max.x - horizExtent),
                linkedCameraPos.y,
                Mathf.Clamp(linkedCameraPos.z, areaBounds.min.z + vertExtent, areaBounds.max.z - vertExtent));
        }
    }
}