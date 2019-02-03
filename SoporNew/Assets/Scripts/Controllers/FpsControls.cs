using UnityEngine;

public class FpsControls : MonoBehaviour
{
    public Transform CameraPivot;
    public float SensivityMult = 0.0f;
    public Vector2 RightFingerInput;
    public float RotationX;
    public float RotationY;
    public int MinimumY = -20;
    public int MaximumY = 20;
    public float SpeedRotation = 25;
    public float TimeRotation = 0.1f;
    public float OriginalRotation;

    private int _rightFingerId;
    private Transform _thisTransform;

    void Start()
    {
        _thisTransform = GetComponent<Transform>();
        OriginalRotation = _thisTransform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (touch.position.x > Screen.width / 2)
                    _rightFingerId = touch.fingerId;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (touch.position.x > Screen.width / 2)
                    if (_rightFingerId == touch.fingerId)
                        RightFingerInput = (touch.deltaPosition + touch.deltaPosition * SensivityMult) * Time.smoothDeltaTime;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (touch.fingerId == _rightFingerId)
                {
                    _rightFingerId = -1;
                    RightFingerInput = new Vector2(0, 0);
                }
            }
        }

        RotationX += RightFingerInput.x * SpeedRotation;
        RotationY += RightFingerInput.y * SpeedRotation;

        RotationY = Mathf.Clamp(RotationY, MinimumY, MaximumY);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, OriginalRotation + RotationX, 0),
            TimeRotation);
        CameraPivot.localRotation = Quaternion.Slerp(CameraPivot.localRotation,
            Quaternion.Euler(CameraPivot.localRotation.x - RotationY, 0, 0), TimeRotation);
    }
}





