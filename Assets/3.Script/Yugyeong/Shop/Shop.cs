using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Shopname
{
    hair = 0,
    riding,
    clothes,
    acc
}

public class Shop : MonoBehaviour
{
    [SerializeField] private Text name_text; //상점 이름

    [SerializeField] private GameObject inapp_obj;//인앱결제
    [SerializeField] private Text inapp_text; 
    [SerializeField] private Text buy_text;

    [SerializeField] private Text money_text;
    [SerializeField] Shop_info shop_Info;
    private int money_;
    public int money
    {
        set
        {
            Update_moneytext();
        }
        get 
        {
            money_ = SQLManager.instance.Item().money;
            Update_moneytext();
            return money_;
        }
    }

    private void OnEnable()
    {
        name_text.text = shop_Info.name;
    }
    private void Buy_item(int price)
    {
        if (price < money)
        {
            //잔액빼기
            SQLManager.instance.Updateitem("money",money-price);
            // 버튼 잠금해제 풀기(재윤아 해줘 넌 천재야)
            //돈잔액 안내
            buy_text.text = $"구매에 성공했습니다.\n 현재잔액 : {money}";
        }
        else
        {
            //현질유도
            
        }
    }

    #region 인앱결제
    private void Complete_purchase()
    {
        inapp_text.text = "인앱결제에 성공했습니다.";
    }

    private void Failed_purchase()
    {
        inapp_text.text = "결제가 취소되었습니다.";
    }

    private void Buy_Gold(int num)
    {
        SQLManager.instance.Updateitem("money", money + num);
    }

    private void Update_moneytext()
    {
        money_text.text = $"{money}";
    }
    #endregion
}
