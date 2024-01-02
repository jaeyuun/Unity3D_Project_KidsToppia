using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : NetworkBehaviour
{
    private FixedJoystick joystick;

    // camera
    [SerializeField] private Transform target = null; // player
    private float axisY;
    private float axisX;

    private float rotSensitive = 3f; // camera rotate sensitive
    private float disZ = 4f;
    private float disY = 1.5f;
    private float rotationMin = -10f;
    private float rotationMax = 80f;
    private float smoothTime = 0.0012f;

    private Vector3 targetRotation;
    private Vector3 currentVel;

    // PC
    private bool isRotate = false;

    private void Start()
    {
        joystick = FindObjectOfType<FixedJoystick>();
        /*if (isLocalPlayer)
        {
            point = target.transform.position;
        }
        else
        {
            gameObject.SetActive(false);
        }*/
    }

    private void LateUpdate()
    {
        RotationCamera();
    }

    private void RotationCamera()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        // touchOn = false;
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                x = Input.touches[0].deltaPosition.x;
                y = Input.touches[0].deltaPosition.y;
                if (Input.GetTouch(0).phase.Equals(TouchPhase.Moved) && !EventSystem.current.IsPointerOverGameObject())
                {
                    Rotate(x, y);
                }
            }
        }
        else
        { // window
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Rotate(x, y);
            }
        }
    }

    private void Rotate(float x, float y)
    {
        Debug.Log("Rotate");
        axisY = axisY + x * rotSensitive; // 좌우 움직임 제어
        axisX = axisX - y * rotSensitive; // 상하 움직임 제어
        axisX = Mathf.Clamp(axisX, rotationMin, rotationMax); // Y축 제한
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(axisX, axisY), ref currentVel, smoothTime);
        this.transform.eulerAngles = targetRotation; // 카메라 회전
        transform.position = target.position - transform.forward * disZ + transform.up * disY;
    }
}
