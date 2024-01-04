using Mirror;
using UnityEngine;

public class LenderCameraController : NetworkBehaviour
{
    // camera
    [SerializeField] private Transform cameraParent;
    public Transform target = null; // player
    public PlayerControll playerControll = null;
    private float axisY = 0;

    private float rotSensitive = 100f; // camera rotate sensitive
    private float disZ = 2.5f;
    private float disY = 1.2f;
    private float smoothTime = 0.2f;

    private Vector3 targetRotation;
    private Vector3 currentVal;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!isLocalPlayer)
        {
            gameObject.SetActive(false);
        }
        else
        {
            cameraParent = GameObject.FindGameObjectWithTag("Camera").transform;
            gameObject.transform.SetParent(cameraParent);
        }
    }

    private void Update()
    {
        if (!isClient) return;
        RotationCamera();
        if (target == null)
        {
            Destroy(gameObject);
        }
    }

    private void RotationCamera()
    {
        float x = Input.GetAxis("Mouse X");
        if (!playerControll.createPanel.activeSelf)
        {
            Quaternion targetRot = Quaternion.AngleAxis(180f, Vector3.up);
            transform.position = target.position - transform.forward * disZ + transform.up * disY;
            transform.rotation = target.rotation * targetRot;
            targetRotation = transform.eulerAngles;
            return;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.phase.Equals(TouchPhase.Began))
                    {
                        if (touch.phase.Equals(TouchPhase.Moved))
                        {
                            x = touch.deltaPosition.x;
                        }
                    }
                }
                Rotate(x);
            }
        }
        else
        { // window
            if (Input.GetMouseButton(0))
            {
                Rotate(x);
            }
        }
    }

    private void Rotate(float x)
    {
        axisY = targetRotation.y + x * rotSensitive; // 좌우 움직임 제어
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(targetRotation.x, axisY), ref currentVal, smoothTime);
        this.transform.eulerAngles = targetRotation; // 카메라 회전
        transform.position = target.position - transform.forward * disZ + transform.up * disY;
    }
}
