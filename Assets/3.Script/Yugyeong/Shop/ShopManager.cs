using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance = null;
    [SerializeField] private Text name_text;

    [SerializeField] private GameObject shop_pannel;
    [SerializeField] private GameObject[] shop_obj;
    [SerializeField] private Shop_slot[] shop_Slots;
    public Shop_slot cur_slot;

    [SerializeField] private GameObject inapp_obj;
    [SerializeField] private GameObject can_inapp;
    [SerializeField] private GameObject go_inapp;
    [SerializeField] private TMP_Text inapp_text;
    [SerializeField] private GameObject inapp_textobj;

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

    public void Set_shop(ShopInfo shopname)
    {
        //���� �̸� ����
        switch (shopname)
        {
            case ShopInfo.Hair:
                name_text.text = "헤어샵";
                break;
            case ShopInfo.Riding:
                name_text.text = "라이딩 가게";
                break;
            case ShopInfo.Clothes:
                name_text.text = "옷가게";
                break;
            case ShopInfo.Acc:
                name_text.text = "악세사리 가게";
                break;
            default:
                name_text.text = "???";
                break;
        }

        //active ���� üũ
        shop_pannel.SetActive(true);
        inapp_obj.SetActive(false);

        //shop_obj �ѱ�
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
            SQLManager.instance.Updateshop(goods.shop, goods.index, 'T');

            //Shop_data shop = SQLManager.instance.Shop();
            buy_text.text = $"구매했습니다.\n 현재 골드 : {money}";
            cur_slot.UI_update();
        }

        else
        {
            Debug.Log($"{goods.price} < {money}�� ���� ����");
            CanPurchase();
        }
    }

    private IEnumerator text_setting(TMP_Text text, string str)
    {
        text.text = $"{str}";
        yield return new WaitForSeconds(3f);
        text.enabled = false;
    }


    #region btn
    public void CanPurchase()//��������
    {
        inapp_obj.SetActive(true);
        can_inapp.SetActive(true);
        go_inapp.SetActive(false);
    }

    public void Can_Yes()
    {
        can_inapp.SetActive(false);
        go_inapp.SetActive(true);
        inapp_textobj.SetActive(false);
    }

    public void Can_No()
    {
        inapp_obj.SetActive(false);
        inapp_textobj.SetActive(false);
    }
    #endregion

    #region 인앱결제
    public void Complete_purchase(int add)
    {
        Debug.Log("Complete_purchase");
        SQLManager.instance.Updateitem("money", money + add);
        text_setting(inapp_text, $"골드가 충전되었습니다.\n 현재 골드 : {SQLManager.instance.Item().money}");
        Update_moneytext();
        inapp_textobj.SetActive(true);
        Invoke("Can_Yes", 3f);
    }

    public void Failed_purchase()
    {
        Debug.Log("Failed_purchase");
        text_setting(inapp_text, "충전에 실패했습니다.");
        Update_moneytext();
        inapp_textobj.SetActive(true);
        Invoke("Can_No", 3f);
    }

    public void Buy_Gold(int num)
    {
        SQLManager.instance.Updateitem("money", money + num);
        Update_moneytext();
        Debug.Log(SQLManager.instance.Item().money);
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
