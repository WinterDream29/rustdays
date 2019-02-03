using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Alien2
{
    public class OrbitCamera2 : MonoBehaviour
    {

        public Transform target;
        public float maxOffsetDistance = 20f;
        public float orbitSpeed = 5f;
        public float panSpeed = .1f;
        public float zoomSpeed = 1f;
        private Vector3 targetOffset = Vector3.zero;

        /// Cam position
        private Vector3 startPos;

        private Vector3 goTo;
        private Quaternion startRot;
        private Renderer rendd;
        private static bool setReset = false;
        private bool autoReset = false;
        private float turnSpeed;

        private Slider mainSlider;
        private bool state;
        private bool curTabb;


        void Start()
        {
            if (target != null) transform.LookAt(target);
            /// Reset Cam position
            startPos = Camera.main.transform.position;
            startRot = Camera.main.transform.rotation;
            mainSlider = GameObject.Find("Canvas").GetComponentInChildren<Slider>();
            mainSlider.value = 30f;
        }


        void Update()
        {
            MoveCam();
            TurnTable();
            NewResetCamPos();
        }




        public void changeState(bool activ)
        {
            state = activ;
        }


        public void setAutoRes(bool activ)
        {
            autoReset = activ;
        }


        public void turnSpeedf(float speed)
        {
            turnSpeed = speed;
            Debug.Log(turnSpeed);
        }


        public void TurnTable()
        {
            if (state)
            {
                Debug.Log(mainSlider.value);
                Vector3 targetPosition = target.position + targetOffset;
                transform.RotateAround(targetPosition, Vector3.up, mainSlider.value * Time.deltaTime);
            }
        }


        public void ResetCamPos()
        {
            GameObject obj = GameObject.Find(GameManager.GetCurrent() + "(Clone)");
            string cur1 = obj.name;

            if (cur1 == "Alien10(Clone)")
            {
                transform.position = new Vector3(0.24f, 2.142f, 2.08f);
                transform.rotation = Quaternion.Euler(16.9f, 186f, 1f);
            }
            else
            {
                targetOffset = Vector3.zero;
                Camera.main.transform.position = startPos;
                Camera.main.transform.rotation = startRot;
            }
        }

        public static void SetNewResetCamPos(bool asd)
        {
            setReset = asd;
        }

        public void SetNewResetCamPos2(bool asd)
        {
            setReset = asd;
        }


        public void NewResetCamPos()
        {
            if (setReset == true)
            {
                GameObject.Find("Canvas").GetComponentsInChildren<Toggle>()[0].isOn = false;
                rendd = GameObject.Find(GameManager.GetCurrent() + "(Clone)").GetComponentInChildren<Renderer>();
                Vector3 center = rendd.bounds.center;
                float radius = rendd.bounds.extents.magnitude;
                goTo = new Vector3(startPos.x, startPos.y, startPos.z + radius - 1.1f);
                transform.LookAt(center);
                transform.LookAt(center);
                transform.position = Vector3.MoveTowards(transform.position, goTo, Time.deltaTime * 1f);

                if (transform.position == goTo)
                {
                    setReset = false;
                    GameObject.Find("Canvas").GetComponentsInChildren<Toggle>()[0].isOn = false;
                }
            }

        }

        public void MoveCam()
        {
            Vector3 targetPosition = target.position + targetOffset;

            /// HardReset Cam position Key
            if (Input.GetKeyDown(KeyCode.Space))
            {
                targetOffset = Vector3.zero;
                Camera.main.transform.position = startPos;
                Camera.main.transform.rotation = startRot;
            }

            ///turntable Key
            if (Input.GetKey(KeyCode.T))
            {
                transform.RotateAround(targetPosition, Vector3.up, 15 * Time.deltaTime);
            }

            // Left Mouse to Orbit
            if (Input.GetMouseButton(0))
            {
                transform.RotateAround(targetPosition, Vector3.up, Input.GetAxis("Mouse X") * orbitSpeed);
                float pitchAngle = Vector3.Angle(Vector3.up, transform.forward);
                float pitchDelta = -Input.GetAxis("Mouse Y") * orbitSpeed;
                float newAngle = Mathf.Clamp(pitchAngle + pitchDelta, 0f, 180f);
                pitchDelta = newAngle - pitchAngle;
                transform.RotateAround(targetPosition, transform.right, pitchDelta);
            }
            // Right Mouse To Truck, Pedestal
            if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
            {
                Vector3 offset = transform.right * -Input.GetAxis("Mouse X") * panSpeed +
                                 transform.up * -Input.GetAxis("Mouse Y") * panSpeed;
                Vector3 newTargetOffset = Vector3.ClampMagnitude(targetOffset + offset, maxOffsetDistance);

                transform.position += newTargetOffset - targetOffset;
                targetOffset = newTargetOffset;
            }
            // Scroll to Zoom
            transform.position += transform.forward * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        }
    }
}