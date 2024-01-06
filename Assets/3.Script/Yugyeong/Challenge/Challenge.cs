using UnityEngine;

public class Challenge : MonoBehaviour
{
    [SerializeField] Element[] elements;

    private void OnEnable()
    {
        elements = transform.GetComponentsInChildren<Element>();
        foreach (Element element in elements)
        {
            element.UI_update();
        }
    }

    public void Get_reward()
    {
        //보상금액
        int reward = gameObject.GetComponentInParent<Element>().challange_data.reward_count;
        Debug.Log($"보상금 : {reward}");

        //기존 금액
        int cur_money = SQLManager.instance.Item(SQLManager.instance.info.User_Id).money;
        Debug.Log($"기존 금액 : {cur_money}");

        //현재 금액 = 보상금액+ 기존금액
        SQLManager.instance.Updateitem("money", reward + cur_money);
        Debug.Log($"보상금 지급 후 소지금액 : {SQLManager.instance.Item(SQLManager.instance.info.User_Id).money}");
    }
}
