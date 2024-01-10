using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance = null;

    [Header("Quiz Menu")]
    public GameObject mainMenu;
    [SerializeField] private TMP_Text animalText;
    [SerializeField] private TMP_Text quizText;

    [Header("Plus Menu")]
    [SerializeField] private GameObject plusMenu;
    [SerializeField] private TMP_Text contentsText;

    [Header("Clear Menu")]
    [SerializeField] private GameObject clearMenu;
    [SerializeField] private GameObject rewardImage;
    [SerializeField] private TMP_Text rewardText;

    [Header("Data")]
    public NPCInfoSetting npcInfoSet; // npc에 따른 동물 이름 정보 가져오기
    List<Dictionary<string, object>> animalData;
    private int dataIndex = 0;
    private int quizIndex = 0;

    private Touch touch;
    [SerializeField] private LayerMask layer;

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
        animalData = CSVReader.Read("AnimalQuiz");
    }

    private void Update()
    {
        Input_touch();
    }

    public void AnimalsQuiz_Print()
    {
        for (int i = 0; i < animalData.Count; i++)
        {
            if (animalData[i]["Animal"].ToString().Trim().Equals(npcInfoSet.npcInfo.npcName.Trim()))
            {
                dataIndex = i;
                break;
            }
        }
        animalText.text = animalData[dataIndex]["Animal"].ToString(); // npc에 따른 animal name
        string[] array = animalData[dataIndex]["Quiz"].ToString().Split(']'); // 해당 animal quiz 순서대로
        quizText.text = array[quizIndex];
        quizIndex++;

        if (quizIndex.Equals(array.Length))
        {
            clearMenu.SetActive(true);
            mainMenu.SetActive(false);
            
        }
    }

    public void Quiz_Result()
    {

    }

    public void Quiz_Reward()
    {

    }

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
}
