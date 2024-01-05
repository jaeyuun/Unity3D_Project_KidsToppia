using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    static public TalkManager instance = null;

    [SerializeField] GameObject talk_pannel;
    [SerializeField] TextMeshProUGUI dialog_text;
    [SerializeField] int dialog_index;
    [SerializeField] TextMeshProUGUI name_text;

    public delegate void del_talkend();
    public static event del_talkend event_talkend;

    [SerializeField] private Touch touch;
    private Vector3 touched_pos;
    private Vector3 mouse_pos;
    [SerializeField] private LayerMask layer;

    [Header("data")]
    List<Dictionary<string, object>> data_Dialog;

    public GameObject npcInfo = null; // 클릭한 NPC 갖고오기 위한 선언, 재윤 24. 01. 05
    [SerializeField] ChatGPT gptResponse;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        data_Dialog = CSVReader.Read("test");
        Close_dialog();
    }

    private void Update()
    {
        Test();
        Input_touch();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Print();
        }
    }

    public void Open_dialog()
    {
        talk_pannel.SetActive(true);
        if (event_talkend != null)
        {
            event_talkend();
        }
    }

    public void Close_dialog()
    {
        talk_pannel.SetActive(false);
        if (event_talkend != null)
        {
            event_talkend();
        }
    }

    public void Print()
    {
        if (!talk_pannel.activeSelf)
        {
            Open_dialog();
        }

        if (dialog_index < data_Dialog.Count)
        {
            name_text.text = data_Dialog[dialog_index]["Character_name"].ToString();
            dialog_text.text = data_Dialog[dialog_index]["Dialog"].ToString();
            dialog_index++;
        }

        else
        {
            Stop_talk();
        }
    }

    public void Stop_talk()
    {
        dialog_index = 0;
        Close_dialog();
    }

    private void Input_touch()
    {
        //Debug.Log("Input_touch");
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    touched_pos = Camera.main.ScreenToWorldPoint(touch.position);
                    Try_raycast(touch.position);
                    StudyManager.instance.Try_raycast(touch.position);
                }
            }
        }

        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Try_raycast(Input.mousePosition);
                StudyManager.instance.Try_raycast(Input.mousePosition);
            }
        }
    }

    private void Try_raycast(Vector3 pos)
    {
        //Debug.Log("Try_raycast");
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            // Debug.Log("레이 쏨");
            if (hit.collider.CompareTag("NPC"))
            {
                npcInfo = hit.collider.gameObject;
                Interactive_NPC();
            }
        }
    }

    private void Interactive_NPC()
    {
        //Debug.Log("NPC찾음");
        Print();
    }
}

[System.Serializable]
public struct TalkData
{
    public string name;
    public string[] contexts;
}
