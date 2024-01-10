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
    int quizIndex = 0;

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
        int dataIndex = 0;

        for (int i = 0; i < animalData.Count; i++)
        {
            if (animalData[i]["Animal"].ToString().Trim().Equals(npcInfoSet.npcInfo.npcName.Trim()))
            {
                dataIndex = i;
                break;
            }
        }
        string[] array = animalData[dataIndex]["Quiz"].ToString().Split(']'); // 해당 animal quiz 순서대로
        contentsText.text = array[quizIndex];

        if (quizIndex.Equals(array.Length))
        {
            clearMenu.SetActive(true);
            mainMenu.SetActive(false);
            quizIndex = 0;
        }
    }

    public void Quiz_Result()
    { // quiz 맞다, 아니다를 누른 뒤 나오는 obj
        int dataIndex = 0;
        for (int i = 0; i < animalData.Count; i++)
        {
            if (animalData[i]["Animal"].ToString().Trim().Equals(npcInfoSet.npcInfo.npcName.Trim()))
            {
                dataIndex = i;
                break;
            }
        }
        string[] array = animalData[dataIndex]["Answer"].ToString().Split(']'); // 해당 animal quiz 순서대로
        quizText.text = array[quizIndex];
    }

    public void Quiz_Result_Button(string answer)
    {
        int dataIndex = 0;
        int rightCount = 0;
        for (int i = 0; i < animalData.Count; i++)
        {
            if (animalData[i]["Animal"].ToString().Trim().Equals(npcInfoSet.npcInfo.npcName.Trim()))
            {
                dataIndex = i;
                break;
            }
        }
        string[] array = animalData[dataIndex]["Info"].ToString().Split(']'); // 해당 animal quiz 순서대로
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].Equals(answer))
            {
                rightCount++;
            }
        }
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
                    if (!mainMenu.activeSelf && !plusMenu.activeSelf && !clearMenu.activeSelf)
                    {
                        Try_raycast(touch.position);
                    }
                    else if (mainMenu.activeSelf)
                    {
                        quizIndex++;
                    }
                    else if (plusMenu.activeSelf)
                    {

                    }
                    else if (clearMenu.activeSelf)
                    {

                    }
                }
            }
        }
        else
        { // Window
            if (Input.GetMouseButtonUp(0))
            {
                if (!mainMenu.activeSelf)
                {
                    Try_raycast(Input.mousePosition);
                }
                else
                {
                    quizIndex++;
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
                AnimalsQuiz_Print();
            }
        }
    }
}
