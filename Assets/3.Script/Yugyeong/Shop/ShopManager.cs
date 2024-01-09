using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public enum Shopname
{
    hair = 0,
    riding,
    clothes,
    acc
}

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance = null;
    [SerializeField] private Text name_text;
    [SerializeField] private Shopname shopname;
    [SerializeField] private GameObject shop_pannel;
    [SerializeField] private GameObject[] shop_obj;
    [SerializeField] private Shop_slot[] shop_Slots;
    public Shop_slot cur_slot;

    [SerializeField] private GameObject inapp_obj;
    [SerializeField] private GameObject can_inapp;
    [SerializeField] private GameObject go_inapp;
    [SerializeField] private Text inapp_text;

    [SerializeField] private GameObject buy_obj;
    [SerializeField] private Text buy_text;

    [SerializeField] private Text money_text;
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
            return money_;
        }
    }
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
        }
    }

    private void Start()
    {
        shop_pannel.SetActive(false);
    }

    public void Taebbi_test() //태삐가 할 예정
    {
        shopname = Shopname.acc;
        Set_shop(shopname);
    }

    private void Set_shop(Shopname shopname)
    {
        //active 상태 체크
        shop_pannel.SetActive(true);
        inapp_obj.SetActive(false);

        //shop_obj 켜기
        for (int i = 0; i < shop_obj.Length; i++)
        {
            if (i == (int)shopname)
            {
                shop_obj[i].SetActive(true);
            }

            else
            {
                shop_obj[i].SetActive(false);
            }
        }

        //slot UI update
        for (int i = 0; i < shop_Slots.Length; i++)
        {
            shop_Slots[i].UI_update();
        }

        //골드 업데이트
        Update_moneytext();
    }

    public void Money(int mon)
    {
        SQLManager.instance.Updateitem("money", mon);
        Update_moneytext();
    }

    public void Buy_item(Goods goods)
    {

        if (goods.price <= money)
        {
            SQLManager.instance.Updateitem("money", money - goods.price);
            // 버튼 잠금해제 풀기(재윤아 해줘 넌 천재야)
            Debug.Log($"{goods.price} <= {money}라서 구매 성공");
            SQLManager.instance.Updateshop(goods.shop, goods.index, 'T');
            var shop = SQLManager.instance.Shop();
            Debug.Log(shop.acc1);
            buy_text.text = $"구매에 성공했습니다.\n 현재골드 : {money}";
            cur_slot.UI_update();
        }
        else
        {
            Debug.Log($"{goods.price} < {money}라서 구매 실패");
            CanPurchase();
        }
    }

    private IEnumerator text_setting(Text text, string str)
    {
        text.text = $"{str}";
        yield return new WaitForSeconds(3f);
        text.enabled = false;
    }


    #region btn
    public void CanPurchase()//현질유도
    {
        inapp_obj.SetActive(true);
        can_inapp.SetActive(true);
        go_inapp.SetActive(false);
    }

    public void Can_Yes()
    {
        can_inapp.SetActive(false);
        go_inapp.SetActive(true);
    }

    public void Can_No()
    {
        inapp_obj.SetActive(false);
    }
    #endregion

    #region 인앱결제
    public void Complete_purchase()
    {
        text_setting(inapp_text, "충전에 성공했습니다.\n현재 골드 : {000}");
        Invoke("Can_No", 3f);
    }

    public void Failed_purchase()
    {
        text_setting(inapp_text, "결제가 취소되었습니다.");
        Invoke("Can_No", 3f);
    }

    public void Buy_Gold(int num)
    {
        SQLManager.instance.Updateitem("money", money + num);
    }

    public void Update_moneytext()
    {
        money_text.text = $"골드 : {money}";
    }
    #endregion
}
public class Shop_data
{
    public string player_id;
    public char riding1;
    public char riding2;
    public char clothes1;
    public char clothes2;
    public char hair1;
    public char hair2;
    public char acc1;
    public char acc2;

    public Shop_data(string playerid, char riding, char riding_, char clothes, char clothes_, char hair, char hair_, char acc, char acc_)
    {
        player_id = playerid;
        riding1 = riding;
        riding2 = riding_;
        clothes1 = clothes;
        clothes2 = clothes_;
        hair1 = hair;
        hair2 = hair_;
        acc1 = acc;
        acc2 = acc_;
    }
}
