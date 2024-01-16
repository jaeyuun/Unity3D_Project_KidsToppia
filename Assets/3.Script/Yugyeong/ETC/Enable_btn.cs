using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enable_btn : MonoBehaviour
{
    [SerializeField] ShopInfo shop;
    [SerializeField] int index;
    [SerializeField] Button btn;
    [SerializeField] Shop_data data;
    // Start is called before the first frame update

    private void OnEnable()
    {
        btn = GetComponent<Button>();
        data = SQLManager.instance.Shop();

        if (data == null)
        {
            return;
        }

        switch (shop)
        {
            case ShopInfo.Hair:
                if (index == 0)
                {
                    Change_click(data.hair1);
                }
                else
                {
                    Change_click(data.hair2);
                }
                break;
            case ShopInfo.Riding:
                if (index == 0)
                {
                    Change_click(data.riding1);
                }
                else
                {
                    Change_click(data.riding2);
                }
                break;
            case ShopInfo.Clothes:
                if (index == 0)
                {
                    Change_click(data.clothes1);
                }
                else
                {
                    Change_click(data.clothes2);
                }
                break;
            case ShopInfo.Acc:
                if (index == 0)
                {
                    Change_click(data.acc1);
                }
                else
                {
                    Change_click(data.acc2);
                }
                break;
            default:
                break;
        }
    }

    private void Change_click(char a)
    {
        if (a == 'T')
        {
            btn.interactable = true;
        }
        else
        {
            btn.interactable = false;
        }
    }
}
