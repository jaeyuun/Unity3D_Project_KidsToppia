using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest_data", order = 0)]
public class Quest_YG : ScriptableObject
{
    [Header("Info")]
    public string info;

    [Header("Reward")]
    //나중에 아이템 만들고 보상 넣어야함

    [Header("Completed")]
    //public List<QuestGoal_YG> goals;
    public QuestGoal_YG goal;
    public QuestCompletedEvent quest_completed;
    public bool iscompleted { get; private set; }

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

[CreateAssetMenu(fileName = "Goal", menuName = "goal_data", order = 1)]
public class QuestGoal_YG : ScriptableObject //퀘스트 목표
{
    public int current_amount { get; protected set; }
    public int required_amount;
    public bool iscompleted { get; private set; }
    public UnityEvent goal_completed;

    public void Initalize()
    {
        iscompleted = false;
        goal_completed = new UnityEvent();
    }

    private void Evaluate()
    {
        if (current_amount >= required_amount)
        {
            Complete();
        }
    }

    private void Complete()
    {
        iscompleted = true;
        goal_completed.Invoke();
        goal_completed.RemoveAllListeners();
    }
}

public class QuestCompletedEvent : UnityEvent<Quest_YG> { }

[Serializable]
public class quest_state_YG //퀘스트 보상
{
    public Sprite sprite;
    public string item_name;
    //public int currency;
    //public int XP;
}