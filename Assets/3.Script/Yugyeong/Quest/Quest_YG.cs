using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Quest_YG", menuName = "YG/Quest_YG/quest_data", order = 0)]
public class Quest_YG : ScriptableObject
{
    [Header("Info")]
    public string info;

    [Header("Reward")]
    //나중에 아이템 만들고 보상 넣어야함

    [Header("Completed")]
    //public List<QuestGoal_YG> goals;
    public Goal_YG goal;
    public QuestCompletedEvent quest_completed;
    public bool iscompleted { get; private set; }

    private void OnEnable()
    {
        Initalize();
    }

    public void Initalize()
    {
        iscompleted = false;
        quest_completed = new QuestCompletedEvent();
        goal.Initalize();
        goal.goal_completed.AddListener(delegate { Check_goals(); });
    }

    private void Check_goals()
    {
        if (iscompleted)
        {
            //보상 주기
            quest_completed.Invoke(this);
            quest_completed.RemoveAllListeners();

            //임시로 박아놓은 퀘스트 완료 debug
            Debug.Log("퀘스트 완료");
        }
    }
}

public class QuestCompletedEvent : UnityEvent<Quest_YG> { }

[Serializable]
public class quest_state_YG //퀘스트 보상 아이템 구조 만들고 다시할예정
{
    public Sprite sprite;
    public string item_name;
    //public int currency;
    //public int XP;
}