using UnityEngine;
using UnityEngine.UI;

public class QuestWindow : MonoBehaviour
{
    //questwindow 요소
    [SerializeField] private Text title_text;
    //[SerializeField] private Text description_text;
    [SerializeField] private QuestGoal_YG goal_prefab;
    [SerializeField] private Transform goal_content;
    //[SerializeField] private Text xp_text;
    //[SerializeField] private Text coin_text;

    public void Initialize(Quest_YG quest)
    {
        //title_text.text = quest.information.name;
        //description_text.text = quest.information.description;
        QuestGoal_YG goal_obj = Instantiate(goal_prefab, goal_content);
        //카운트 불러오는 방법 생각하기
        //GameObject count_obj = goal_obj.gameObject.transform.Find("Count").gameObject;

        if (goal_prefab.iscompleted)
        {
            //count_obj.SetActive(false);
            //goal_obj.transform.Find("Done").gameObject.SetActive(true);
        }
        else
        {
            //count_obj.GetComponent<Text>().text = goal.current_amount + "/" + goal.required_amount;
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < goal_content.childCount; i++)
        {
            Destroy(goal_content.GetChild(i).gameObject);
        }
    }
}
