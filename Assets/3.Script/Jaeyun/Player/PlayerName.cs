using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerName : NetworkBehaviour
{
    [SerializeField] private PlayerCreate player;
    public PlayerCreate target_info;
    [SerializeField] private PlayerName target_player;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject click_obj;      

    [SerializeField] private Touch touch;
    [SerializeField] private LayerMask layer;

    private void Start()
    {
        click_obj.SetActive(false);
    }
    private void FixedUpdate()
    {
        PlayerNameUpdate();
    }

    private void Update()
    {
        Find_Player();

        if (click_obj.activeSelf)
        {
            ClickobjUpdate();
        }
    }

    public void PlayerNameSet()
    {
        nameText.text = player.info.User_NickName;
    }

    public void Set_clickobj()
    {
        Debug.Log(gameObject.name);
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
        {
            Debug.Log("Input_touch");
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount > 0)
                {
                    touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        Try_raycast(touch.position);
                        StudyManager.instance.Try_raycast(touch.position);
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
    }

    private void Try_raycast(Vector3 pos)
    {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            if (hit.collider.CompareTag("Player"))
            {

                Debug.Log("asdf");
                target_player = hit.collider.gameObject.transform.root.GetComponent<PlayerName>();
                target_info = hit.collider.gameObject.transform.root.GetComponent<PlayerCreate>();
                target_player.Set_clickobj();
            }
        }
    }

    private void PlayerNameUpdate()
    {
        nameText.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2f, 0));
    }

    private void ClickobjUpdate()
    {
        click_obj.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3f, 0));
    }
}