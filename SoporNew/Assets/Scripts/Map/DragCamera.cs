using UnityEngine;

namespace Assets.Scripts.Map
{
    public class DragCamera : MonoBehaviour
    {
        //public float DragSpeed = 2;

        //private Vector3 _dragOrigin;
        //public bool CameraDragging = true;

        //void Update ()
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        _dragOrigin = Input.mousePosition;
        //        return;
        //    }

        //    if (!Input.GetMouseButton(0)) return;

        //    Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _dragOrigin);
        //    Vector3 move = new Vector3(pos.x * DragSpeed, 0, pos.y * DragSpeed);

        //    transform.Translate(move, Space.World);
        //}

        public float turnSpeed = 35.0f;         // Speed of camera turning when mouse moves in along an axis
        public float panSpeed = 360.0f;         // Speed of the camera when being panned
        public float zoomSpeed = 500.0f;        // Speed of the camera going back and forth

        public float turnDrag = 5.0f;           // RigidBody Drag when rotating camera
        public float panDrag = 3.5f;            // RigidBody Drag when panning camera
        public float zoomDrag = 3.3f;           // RigidBody Drag when zooming camera

        private Vector3 mouseOrigin;            // Position of cursor when mouse dragging starts
        private bool isPanning;             // Is the camera being panned?
        private bool isRotating;            // Is the camera being rotated?
        private bool isZooming;             // Is the camera zooming?

        private Rigidbody _rigidbody;

        //
        // AWAKE
        //

        void Awake()
        {
            // Setup camera physics properties
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.useGravity = false;
        }

        //
        // UPDATE: For input
        //

        void Update()
        {
            // == Getting Input ==

            // Get the left mouse button
            //if (Input.GetMouseButtonDown(1))
            //{
            //    // Get mouse origin
            //    mouseOrigin = Input.mousePosition;
            //    isRotating = true;
            //}

            // Get the right mouse button
            if (Input.GetMouseButtonDown(0))
            {
                // Get mouse origin
                mouseOrigin = Input.mousePosition;
                isPanning = true;
            }

            // Get the middle mouse button
            //if (Input.GetMouseButtonDown(2))
            //{
            //    // Get mouse origin
            //    mouseOrigin = Input.mousePosition;
            //    isZooming = true;
            //}


            // == Disable movements on Input Release ==

            if (!Input.GetMouseButton(0)) isPanning = false;
            if (!Input.GetMouseButton(1)) isRotating = false;
            if (!Input.GetMouseButton(2)) isZooming = false;

        }

        //
        // Fixed Update: For Physics
        //

        void FixedUpdate()
        {
            // == Movement Code ==

            // Rotate camera along X and Y axis
            if (isRotating)
            {
                // Get mouse displacement vector from original to current position
                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

                // Set Drag
                _rigidbody.angularDrag = turnDrag;

                // Two rotations are required, one for x-mouse movement and one for y-mouse movement
                _rigidbody.AddTorque(-pos.y * turnSpeed * transform.right, ForceMode.Acceleration);
                _rigidbody.AddTorque(pos.x * turnSpeed * transform.up, ForceMode.Acceleration);
            }

            // Move (pan) the camera on it's XY plane
            if (isPanning)
            {
                // Get mouse displacement vector from original to current position
                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
                Vector3 move = new Vector3(pos.x * panSpeed, 0, pos.y * panSpeed);

                // Apply the pan's move vector in the orientation of the camera's front
                Quaternion forwardRotation = Quaternion.LookRotation(transform.forward, transform.up);
               // move = forwardRotation * move;

                // Set Drag
                _rigidbody.drag = panDrag;

                // Pan
                _rigidbody.AddForce(move, ForceMode.Acceleration);
            }

            // Move the camera linearly along Z axis
            if (isZooming)
            {
                // Get mouse displacement vector from original to current position
                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
                Vector3 move = pos.y * zoomSpeed * transform.forward;

                // Set Drag
                _rigidbody.drag = zoomDrag;

                // Zoom
                _rigidbody.AddForce(move, ForceMode.Acceleration);
            }
        }
    }
}