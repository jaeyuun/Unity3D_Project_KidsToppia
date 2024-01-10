using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    static public TalkManager instance = null;

    [Header("Talk Panel")]
    [SerializeField] TextMeshProUGUI dialog_text;
    [SerializeField] GameObject talk_pannel;
    [SerializeField] TextMeshProUGUI name_text;
    private int dialog_index;

    [Header("Button in Talk Panel")]
    [SerializeField] private GameObject yes_button;
    [SerializeField] private TextMeshProUGUI yes_button_text;
    [SerializeField] private TextMeshProUGUI no_button_text;
    [SerializeField] private GameObject micButton; // audio button

    public delegate void del_talkend();
    public static event del_talkend event_talkend;

    [SerializeField] private Touch touch;
    [SerializeField] private LayerMask layer;

    [Header("data")]
    List<Dictionary<string, object>> data_Dialog;

    [Header("NPC")]
    public NPCInfoSetting npcInfoSet; // 클릭한 NPC 갖고오기 위한 선언, 재윤 24. 01. 05
    [SerializeField] private ChatGPT gptResponse; // npc에 따라 요청하는 response
    private bool nextText = false;

    // user touch position
    private Vector3 touched_pos;
    private Vector3 mouse_pos;

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
            return;
        }
    }
    private void Start()
    {
        data_Dialog = CSVReader.Read("test");
        Close_dialog();
    }

    private void Update()
    {
        Input_touch();
    }

    #region Dialog Pannel Active
    public void Open_dialog()
    {
        talk_pannel.SetActive(true);
        micButton.SetActive(false); // 맨 처음 talkpanel을 열었을 때는 micbutton 없음
        // npc info setting, button text
        no_button_text.text = $"{npcInfoSet.npcInfo.noButtonText}";
        yes_button_text.text = $"{npcInfoSet.npcInfo.yesButtonText}";
        yes_button.SetActive(npcInfoSet.npcInfo.npcYesButton); // npc에 따른 talk panel button 개수
        nextText = true;

        if (event_talkend != null)
        {
            event_talkend();
        }
    }

    public void Close_dialog()
    { // No_Button Click method
        talk_pannel.SetActive(false);
        nextText = false;
        if (event_talkend != null)
        {
            event_talkend();
        }
    }
    #endregion

    public void DialogText_Print()
    { // dialog pannel 
        if (!talk_pannel.activeSelf)
        {
            Open_dialog();
        }

        // npc info에 따른 text 출력
        for (int i = 0; i < data_Dialog.Count; i++)
        {
            if (data_Dialog[i]["Character_name"].ToString().Trim().Equals(npcInfoSet.npcInfo.npcName.Trim()))
            {
                dialog_index = i;
                break;
            }
        }
        name_text.text = data_Dialog[dialog_index]["Character_name"].ToString();
        string[] array = data_Dialog[dialog_index]["Dialog"].ToString().Split(']');
        dialog_text.text = array[Random.Range(0, array.Length)];
    }

    public void Stop_talk()
    {
        Close_dialog();
    }
    #region NPC 구분
    private void Input_touch()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                {
                    if (!talk_pannel.activeSelf)
                    {
                        touched_pos = Camera.main.ScreenToWorldPoint(touch.position);
                        Try_raycast(touch.position);
                        StudyManager.instance.Try_raycast(touch.position);
                    }
                    else
                    {
                        if (nextText)
                        { // npc info text
                            dialog_text.text = $"<#ABABAB>{npcInfoSet.npcInfo.npcText}</color>";
                            micButton.SetActive(npcInfoSet.npcInfo.micButton); // audio button을 사용하는 지
                        }
                    }
                }
            }
        }

        else
        { // Window
            if (Input.GetMouseButtonUp(0))
            {
                if (!talk_pannel.activeSelf)
                {
                    mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Try_raycast(Input.mousePosition);
                    StudyManager.instance.Try_raycast(Input.mousePosition);
                }
                else
                {
                    if (nextText)
                    { // npc info text
                        dialog_text.text = $"<#ABABAB>{npcInfoSet.npcInfo.npcText}</color>";
                        micButton.SetActive(npcInfoSet.npcInfo.micButton); // audio button을 사용하는 지
                    }
                }
            }
        }
    }

    private void Try_raycast(Vector3 pos)
    { // NPC 찾는 raycast
        Ray ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
        {
            if (hit.collider.CompareTag("NPC"))
            { // npc일 때
                npcInfoSet = hit.collider.gameObject.GetComponent<NPCInfoSetting>();
                QuizManager.instance.npcInfoSet = npcInfoSet; // npc info Quiz manager에게 전달
                DialogText_Print();
            }
        }
    }

    public void YesButton()
    {
        if (npcInfoSet.npcInfo.npcName.Equals("유삐"))
        { // Quiz
            QuizManager.instance.mainMenu.SetActive(true);
        }
        else if (npcInfoSet.npcInfo.npcName.Equals("태삐"))
        { // Shop
            // 나중에 SetActive(ture) 꼬옥 추가해주면돼,,, todo
        }
    }
    #endregion
}

[System.Serializable]
public struct TalkData
{
    public string name;
    public string[] contexts;
}
