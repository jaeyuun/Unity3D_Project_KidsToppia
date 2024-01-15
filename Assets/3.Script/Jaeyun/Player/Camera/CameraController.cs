using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CameraController : NetworkBehaviour
{
    // camera
    [SerializeField] private Transform cameraParent;
    public Transform target = null; // player
    public PlayerControll playerControll = null;
    private float axisY;
    private float axisX;

    private float rotSensitive = 100f; // camera rotate sensitive
    private float disZ = 4f;
    private float disY = 1.5f;
    private float rotationMin = -10f;
    private float rotationMax = 80f;
    private float smoothTime = 0.2f;

    private Vector3 targetRotation;
    private Vector3 currentVel;

    private bool isRotate = false;
    private bool[] touchOn = new bool[20];

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!isLocalPlayer)
        {
            gameObject.SetActive(false);
        }
        else
        {
            TalkManager.instance.mainCamera = this.gameObject;
            if (Application.platform != RuntimePlatform.Android)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                rotSensitive = 2f;
                for (int i = 0; i < touchOn.Length; i++)
                {
                    touchOn[i] = false;
                }
            }
            target = playerControll.transform;
            transform.position = target.position - transform.forward * disZ + transform.up * disY;
            transform.rotation = target.rotation;
            cameraParent = GameObject.FindGameObjectWithTag("Camera").transform;
            gameObject.transform.SetParent(cameraParent);
        }
    }

    private void LateUpdate()
    {
        if (isLocalPlayer)
        {
            MoveCamera();
            RotationCamera();
        }
        if (target == null)
        {
            Destroy(gameObject);
        }
    }

    private void MoveCamera()
    {
        transform.position = target.position - transform.forward * disZ + transform.up * disY;
    }

    private void RotationCamera()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        if (playerControll.createPanel.activeSelf) return;
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject())
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.phase.Equals(TouchPhase.Ended))
                    {
                        touchOn[touch.fingerId] = false;
                    }
                    if (IsPointerOverUIObject(touch))
                    {
                        touchOn[touch.fingerId] = true;
                        continue;
                    }
                    if (touch.phase.Equals(TouchPhase.Moved) && !touchOn[touch.fingerId])
                    {
                        x = touch.deltaPosition.x;
                        y = touch.deltaPosition.y;
                        isRotate = true;
                    }
                    else
                    {
                        isRotate = false;
                    }
                }
                if (isRotate)
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
        axisY = targetRotation.y + x * rotSensitive; // 좌우 움직임 제어
        axisX = targetRotation.x - y * rotSensitive; // 상하 움직임 제어
        axisX = Mathf.Clamp(axisX, rotationMin, rotationMax); // Y축 제한
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(axisX, axisY), ref currentVel, smoothTime);
        this.transform.eulerAngles = targetRotation; // 카메라 회전
        transform.position = target.position - transform.forward * disZ + transform.up * disY;
    }

    private bool IsPointerOverUIObject(Touch touch)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(touch.position.x, touch.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
