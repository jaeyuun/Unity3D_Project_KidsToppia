using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance = null;

    [Header("Quiz Menu")]
    public GameObject quizCanvas;
    public GameObject mainMenu;
    [SerializeField] private TMP_Text animalText;
    [SerializeField] private TMP_Text quizText;

    [Header("Plus Menu")]
    [SerializeField] private GameObject plusMenu;
    [SerializeField] private GameObject[] answerImage;
    [SerializeField] private TMP_Text contentsText;

    [Header("Clear Menu")]
    [SerializeField] private GameObject clearMenu;
    [SerializeField] private GameObject[] rewardImage;
    [SerializeField] private TMP_Text rewardText;

    [Header("Data")]
    public NPCInfoSetting npcInfoSet; // npc에 따른 동물 이름 정보 가져오기
    List<Dictionary<string, object>> animalData;

    // animal quiz csv file data
    int dataIndex = 0; // animal index
    int quizIndex = 0; // animal의 quiz 순서
    int rightCount = 0; // quiz 맞은 개수

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

    public void AnimalsQuiz_Print()
    {
        for (int i = 0; i < animalData.Count; i++)
        {
            if (animalData[i]["Animal_Id"].ToString().Trim().Equals(npcInfoSet.npcInfo.animal_id.Trim()))
            {
                dataIndex = i;
                break;
            }
        }
        animalText.text = animalData[dataIndex]["Animal"].ToString(); // animal name
        string[] array = animalData[dataIndex]["Quiz"].ToString().Split(']'); // 해당 animal quiz 순서대로
        quizText.text = array[quizIndex];
        if (mainMenu.activeSelf)
        {
            mainMenu.SetActive(true);
        }
        if (quizIndex.Equals(array.Length))
        {
            clearMenu.SetActive(true);
            mainMenu.SetActive(false);
        }
    }

    public void Quiz_Result()
    { // quiz
        string[] array = animalData[dataIndex]["Answer"].ToString().Split(']'); // 정답 확인 설명
        contentsText.text = array[quizIndex];
    }

    public void NextButton()
    {
        quizIndex++;
        if (quizIndex == 3)
        {
            mainMenu.SetActive(false);
            clearMenu.SetActive(true);
            Quiz_Reward();
        }
        else
        {
            AnimalsQuiz_Print();
        }
    }

    public void EndButton()
    {
        // 퀴즈가 끝난 뒤 초기화
        quizIndex = 0;
        dataIndex = 0;
        quizIndex = 0;
        rightCount = 0;

        for (int i = 0; i < rewardImage.Length; i++)
        {
            rewardImage[i].SetActive(false);
        }

        mainMenu.SetActive(true);
        clearMenu.SetActive(false);
    }

    public void Quiz_Result_Button(string answer)
    { // 맞다! Button이면 answer O, 아니다! Button이면 X 
        Quiz_Result();
        plusMenu.SetActive(true);
        string[] array = animalData[dataIndex]["Info"].ToString().Split(']'); // O or X
        if (array[quizIndex].Equals(answer))
        {
            answerImage[0].SetActive(true);
            answerImage[1].SetActive(false);
            rightCount++;
        }
        else
        {
            answerImage[0].SetActive(false);
            answerImage[1].SetActive(true);
        }
    }

    public void Quiz_Reward()
    {
        for (int i = 0; i < rightCount; i++)
        {
            rewardImage[i].SetActive(true);
        }
        rewardText.text = $"다시 도전 시 보상획득";
        Nonplayer_data userQuizData = SQLManager.instance.Collection(SQLManager.instance.info.User_Id, "issolved"); // 문제 푼 것 확인
        if (userQuizData.is_solved.Equals('T'))
        {
            rewardText.text = "이미 보상을 받았어요";
        }
        else
        {
            if (rightCount.Equals(3))
            {
                SQLManager.instance.Updatecollection(animalData[dataIndex]["Animal_Id"].ToString(), "issolved", 'T'); // 문제를 전부 맞췄을 경우 T로 바꾸어줌
                rewardText.text = $"+300개 보상";
                SQLManager.instance.Updateitem("money", 300);
                SQLManager.instance.Updateitem("food_num", 2);
            }
            else
            {
                rewardText.text = $"다시 도전 시 보상획득";
            }
        }
    }
}
