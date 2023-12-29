using System.Collections;
using UnityEngine;
using UnityEngine.Events;

interface IGoal_action
{
    void Action();
}

interface IGoal_item
{
    void Item();
}
[CreateAssetMenu(fileName = "Goal_YG"  , menuName = "YG/Quest_YG/Goal_YG", order = 3)]
public class Goal_YG : ScriptableObject, IGoal_action
{
    [SerializeField] private int current_amount;
    [SerializeField] private int required_amount;
    public bool iscompleted;
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

    private void Up_current()
    {
        current_amount++;
        Evaluate();
    }

    private void Complete()
    {
        iscompleted = true;
        goal_completed.Invoke();
        goal_completed.RemoveAllListeners();
    }

    public void Action()
    {
        
    }
}

[CreateAssetMenu(fileName = "Goal_action_YG", menuName = "YG/Quest_YG/Goal_action_YG", order = 1)]
public class Goal_action_YG : Goal_YG, IGoal_action
{
    //public void Action()
    //{
    //    throw new System.NotImplementedException();
    //}
}

[CreateAssetMenu(fileName = "Goal_item_YG", menuName = "YG/Quest_YG/Goal_item_YG", order = 2)]
public class Goal_item_YG : Goal_YG, IGoal_item
{
    public void Item()
    {
        throw new System.NotImplementedException();
    }
}
