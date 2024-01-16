using Mirror;
using TMPro;
using UnityEngine;

public class PlayerName : NetworkBehaviour
{
    [SerializeField] private PlayerCreate player;
    public PlayerCreate target_info;
    [SerializeField] private PlayerName target_player;

    public TMP_Text nameText;
    [SerializeField] private GameObject click_obj;

    [SerializeField] private Touch touch;
    [SerializeField] private LayerMask layer_player;
    [SerializeField] private LayerMask layer_playerLocal;

    private void Start()
    {
        click_obj.SetActive(false);
        if (isLocalPlayer)
        {
            nameText.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        PlayerNameUpdate();
    }

    private void Update()
    {
        if (click_obj.activeSelf)
        {
            ClickobjUpdate();
        }
        if (!isLocalPlayer) return;
        Find_Player();
    }

    public void PlayerNameSet()
    {
        nameText.text = player.info.User_NickName;
    }

    public void Set_clickobj()
    {
        if (click_obj.activeSelf)
        {
            click_obj.SetActive(false);
        }
        else
        {
            click_obj.SetActive(true);
        }
    }

    public void Find_Player()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Try_raycast(touch.position);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                //mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Try_raycast(Input.mousePosition);
            }
        }
    }

    private void Try_raycast(Vector3 pos)
    {
        if (!TalkManager.instance.mainCamera.activeSelf) return;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer_player))
        {
            if (hit.collider.CompareTag("Player"))
            {
                target_player = hit.collider.gameObject.transform.root.GetComponent<PlayerName>();
                target_info = hit.collider.gameObject.transform.root.GetComponent<PlayerCreate>();
                target_player.Set_clickobj();
            }
        }
    }

    private void PlayerNameUpdate()
    {
        if (!isLocalPlayer)
        { // player가 뒤에 있거나 거리가 멀어질 경우 출력 X
            if (!Physics.CheckSphere(transform.position, 20f, layer_playerLocal) || nameText.gameObject.transform.position.z < 0)
            {
                nameText.enabled = false;
            }
            else
            {
                nameText.enabled = true;
            }
        }

        if (TalkManager.instance.mainCamera == null) return;
        if (!TalkManager.instance.mainCamera.activeSelf)
        {
            nameText.gameObject.SetActive(false);
        }
        else
        {
            nameText.gameObject.SetActive(true);
            nameText.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2f, 0));
        }
    }

    private void ClickobjUpdate()
    {
        click_obj.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2.5f, 0));
    }
}