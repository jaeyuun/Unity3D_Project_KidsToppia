using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : NetworkBehaviour
{
    // camera
    [SerializeField] private Transform target = null; // player
    [SerializeField] private PlayerControll playerControll = null;
    private float axisY;
    private float axisX;

    private float rotSensitive = 3f; // camera rotate sensitive
    private float disZ = 4f;
    private float disY = 1.5f;
    private float rotationMin = -10f;
    private float rotationMax = 80f;
    private float smoothTime = 0.2f;

    private Vector3 targetRotation;
    private Vector3 currentVel;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
            playerControll = FindObjectOfType<PlayerControll>();
            target = playerControll.transform;
            this.transform.eulerAngles = targetRotation; // 카메라 회전
            transform.position = target.position - transform.forward * disZ + transform.up * disY;
        }
    }

    private void LateUpdate()
    {
        if (isLocalPlayer)
        {
            RotationCamera();
        }
    }

    private void RotationCamera()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject())
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.phase.Equals(TouchPhase.Began))
                    {
                        if (touch.phase.Equals(TouchPhase.Moved) && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            x = Input.touches[i].deltaPosition.x;
                            y = Input.touches[i].deltaPosition.y;
                        }
                    }
                }
                Rotate(x, y);
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
        axisY = axisY + x * rotSensitive; // 좌우 움직임 제어
        axisX = axisX - y * rotSensitive; // 상하 움직임 제어
        axisX = Mathf.Clamp(axisX, rotationMin, rotationMax); // Y축 제한
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(axisX, axisY), ref currentVel, smoothTime);
        this.transform.eulerAngles = targetRotation; // 카메라 회전
        transform.position = target.position - transform.forward * disZ + transform.up * disY;
    }
}
